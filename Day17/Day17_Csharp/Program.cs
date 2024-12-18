namespace Day17_Csharp;

public class Program
{
    public static void Main(string[] args)
    {
        const bool isDev = false;
        var lines = File.ReadAllLines(isDev ? "example.txt" : "input.txt");
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

        Console.WriteLine("Processing input...");
        var result = OpcodeProcessor.Run(input);
        Console.WriteLine($"Output: {result}");
    }
}

public static class OpcodeProcessor
{
    private static int rA, rB, rC, p;
    private static List<int> oprs;

    public static string Run(InputData input)
    {
        var res = new List<int>();
        Reset(input);

        while (p < oprs.Count)
        {
            var opcode = oprs[p];

            if (p + 1 >= oprs.Count)
                return string.Join(",", res);

            var opand = oprs[p + 1];

            switch (opcode)
            {
                case 0:
                    HandleAdv(opand);
                    break;
                case 1:
                    HandleBxl(opand);
                    break;
                case 2:
                    HandleBst(opand);
                    break;
                case 3:
                    HandleJnz(opand);
                    break;
                case 4:
                    HandleBxc(opand);
                    break;
                case 5:
                    res.Add(HandleOut(opand));
                    break;
                case 6:
                    HandleBdv(opand);
                    break;
                case 7:
                    HandleCdv(opand);
                    break;
            }

            if (opcode != 3 || rA == 0)
                p += 2;
        }

        return string.Join(",", res);
    }


    private static void HandleAdv(int opand)
    {
        rA = (int)Math.Truncate((double)rA / Dv(opand));
    }

    private static void HandleBxl(int opand)
    {
        rB ^= opand;
    }

    private static void HandleBst(int opand)
    {
        rB = GetCombo(opand) & 7;
    }

    private static void HandleJnz(int opand)
    {
        if (rA != 0)
            p = opand;
    }

    private static void HandleBxc(int opand)
    {
        rB ^= rC;
    }

    private static int HandleOut(int opand)
    {
        return GetCombo(opand) & 7;
    }

    private static void HandleBdv(int opand)
    {
        rB = (int)Math.Truncate((double)rA / Dv(opand));
    }

    private static void HandleCdv(int opand)
    {
        rC = (int)Math.Truncate((double)rA / Dv(opand));
    }

    private static int Dv(int opand)
    {
        return (int)Math.Pow(2, GetCombo(opand));
    }

    private static int GetCombo(int opand)
    {
        return opand switch
        {
            4 => rA,
            5 => rB,
            6 => rC,
            _ => opand
        };
    }

    private static void Reset(InputData input)
    {
        p = 0;
        rA = input.RA;
        rB = input.RB;
        rC = input.RC;
        oprs = [..input.Opr];
    }
}

public class InputData
{
    public int RA { get; set; }
    public int RB { get; set; }
    public int RC { get; set; }
    public List<int> Opr { get; set; }
}