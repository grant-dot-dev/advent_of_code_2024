import math

BUTTON_A_COST = 3
BUTTON_B_COST = 1
PART_1 = False
OFFSET = 0 if PART_1 else 10000000000000


def process(filename: str) -> int:
    lines = []
    with open(filename, "r") as file:
        for line in file:
            lines.append(line.strip())
    return process_lines(lines)


def process_lines(lines: list[str]) -> int:
    total_cost = 0

    for i in range(0, len(lines), 4):
        aline = lines[i]
        bline = lines[i + 1]
        pline = lines[i + 2]

        total_cost += process_line(aline, bline, pline)

    return total_cost


def get_xy(line: str) -> tuple[int, int]:
    parts = line.split(":")[1].strip().split(",")
    x = int(parts[0].split("+")[1].strip())
    y = int(parts[1].split("+")[1].strip())
    return x, y


def get_prize(line: str) -> tuple[int, int]:
    parts = line.split(":")[1].strip().split(",")
    x = OFFSET + int(parts[0].split("=")[1].strip())
    y = OFFSET + int(parts[1].split("=")[1].strip())
    return x, y


def process_line(aline: str, bline: str, pline: str) -> int:
    a_vals = get_xy(aline)
    b_vals = get_xy(bline)
    prize_vals = get_prize(pline)

    a1, b1, c1 = a_vals[0], b_vals[0], prize_vals[0]
    a2, b2, c2 = a_vals[1], b_vals[1], prize_vals[1]

    # Calculate determinants using Cramer's Rule
    D = a1 * b2 - a2 * b1  # Main determinant
    Dx = c1 * b2 - c2 * b1  # Determinant for A
    Dy = a1 * c2 - a2 * c1  # Determinant for B

    if D == 0:
        return 0  # No unique solution

    # Solve for A and B
    A = Dx / D
    B = Dy / D

    # Check for integer solutions
    if not (A.is_integer() and B.is_integer()):
        return 0

    # Calculate total cost
    A, B = int(A), int(B)
    return A * BUTTON_A_COST + B * BUTTON_B_COST


# Example usage:
if __name__ == "__main__":
    filename = "input.txt"
    total_cost = process(filename)
    print(f"Total Cost: {total_cost}")
