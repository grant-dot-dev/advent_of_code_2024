namespace Day17_Csharp;

public class Program
{
    public static void Main(string[] args)
    {
        var part1 = new Part1();
        part1.Solve(true);

        var part2 = new Part2(true);
        part2.Solve();
    }
}

public class Part1
{
    public void Solve(bool isProduction)
    {
        var lines = File.ReadAllLines(!isProduction ? "example.txt" : "input.txt");
        var regA = int.Parse(lines[0].Split(": ")[1]);
        var regB = int.Parse(lines[1].Split(": ")[1]);
        var regC = int.Parse(lines[2].Split(": ")[1]);
        var operations = lines[4].Split(": ")[1].Split(",").Select(int.Parse).ToList();


        var input = new InputData()
        {
            RA = regA,
            RB = regB,
            RC = regC,
            Opr = operations
        };

        var result = OpcodeProcessor.Run(input);
        Console.WriteLine($"Output Pt1: {result}");
    }
}

public class Part2()
{
    private readonly List<int> _program = [];

    public Part2(bool isProduction) : this()
    {
        var lines = File.ReadAllLines(!isProduction ? "example.txt" : "input.txt");
        _program = lines[4].Split(": ")[1].Split(",").Select(int.Parse).ToList();
    }

    public void Solve()
    {
        var result = Find(_program, 0);
        Console.WriteLine(result.HasValue ? $"Output Pt2: {result.Value}" : "No solution found");
    }

    private long? Find(List<int> target, long ans)
    {
        if (target.Count == 0)
        {
            return ans;
        }

        for (var t = 0; t < 8; t++)
        {
            long a = (ans << 3) | (uint)t;
            long b = 0;
            long c = 0;
            int? output = null;
            bool adv3 = false;

            for (var pointer = 0; pointer < _program.Count - 2; pointer += 2)
            {
                var ins = _program[pointer];
                var operand = _program[pointer + 1];

                switch (ins)
                {
                    case 0:
                        if (adv3)
                            throw new Exception("program has multiple ADVs");
                        if (operand != 3)
                            throw new Exception("program has ADV with operand other than 3");
                        adv3 = true;
                        break;
                    case 1:
                        b ^= operand;
                        break;
                    case 2:
                        b = Combo(operand) % 8;
                        break;
                    case 3:
                        throw new Exception("program has JNZ inside expected loop body");
                    case 4:
                        b ^= c;
                        break;
                    case 5:
                        if (output.HasValue)
                            throw new Exception("program has multiple OUT");
                        output = (int)(Combo(operand) % 8);
                        break;
                    case 6:
                        b = a >> (int)Combo(operand);
                        break;
                    case 7:
                        c = a >> (int)Combo(operand);
                        break;
                }

                if (!output.HasValue || output.Value != target[^1]) continue;

                var newTarget = target.Take(target.Count - 1).ToList();
                var sub = Find(newTarget, a);
                if (!sub.HasValue)
                    continue;
                return sub;
            }

            continue;

            long Combo(int operand)
            {
                return operand switch
                {
                    >= 0 and <= 3 => operand,
                    4 => a,
                    5 => b,
                    6 => c,
                    _ => throw new ArgumentException($"unrecognized combo operand {operand}")
                };
            }
        }

        return null;
    }
}

public static class OpcodeProcessor
{
    private static int _rA, _rB, _rC, _p;
    private static List<int> _oprs;

    public static string Run(InputData input)
    {
        var res = new List<int>();
        Reset(input);

        while (_p < _oprs.Count)
        {
            var opcode = _oprs[_p];

            if (_p + 1 >= _oprs.Count)
                return string.Join(",", res);

            var opand = _oprs[_p + 1];

            switch (opcode)
            {
                case 0:
                    _rA = (int)Math.Truncate((double)_rA / (int)Math.Pow(2, GetCombo(opand)));
                    break;
                case 1:
                    _rB ^= opand;
                    break;
                case 2:
                    _rB = GetCombo(opand) & 7;
                    break;
                case 3:
                    if (_rA != 0)
                        _p = opand;
                    break;
                case 4:
                    _rB ^= _rC;
                    break;
                case 5:
                    res.Add(GetCombo(opand) & 7);
                    break;
                case 6:
                    _rB = (int)Math.Truncate((double)_rA / (int)Math.Pow(2, GetCombo(opand)));
                    break;
                case 7:
                    _rC = (int)Math.Truncate((double)_rA / (int)Math.Pow(2, GetCombo(opand)));
                    break;
            }

            if (opcode != 3 || _rA == 0)
                _p += 2;
        }

        return string.Join(",", res);
    }


    private static int GetCombo(int opand)
    {
        return opand switch
        {
            4 => _rA,
            5 => _rB,
            6 => _rC,
            _ => opand
        };
    }

    private static void Reset(InputData input)
    {
        _p = 0;
        _rA = input.RA;
        _rB = input.RB;
        _rC = input.RC;
        _oprs = [.. input.Opr];
    }
}

public class InputData
{
    public int RA { get; set; }
    public int RB { get; set; }
    public int RC { get; set; }
    public List<int> Opr { get; set; }
}