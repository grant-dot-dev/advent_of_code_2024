def main():
    # Read the input file and split each line into a list of strings
    with open("./input1.txt") as file:
        reports = [line.split() for line in file]

    part1(reports)
    part2(reports)


def part1(reports):
    # Count valid reports using a list comprehension and the IsValidReport function

    valid_reports = 0
    for report in reports:
        levels = list(map(int, report))

        if is_valid_report(levels):
            valid_reports += 1

    print(valid_reports)


def part2(reports):
    valid_reports = 0

    for report in reports:
        ints = list(map(int, report))

        # If valid as-is, count it
        if is_valid_report(ints):
            valid_reports += 1
            continue

        # Check if removing any single level makes it valid
        valid_with_dampener = False

        # Iterate over each element in ints
        for i in range(len(ints)):
            # Create a new list without the element at index i
            modified_report = [x for j, x in enumerate(ints) if j != i]

            # Check if the modified report is valid
            if is_valid_report(modified_report):
                valid_with_dampener = True
                break

        if valid_with_dampener:
            valid_reports += 1

    print(valid_reports)


def is_valid_report(levels):
    # Check if the sequence is either all increasing or all decreasing
    is_increasing = is_increasing_sequence(levels)
    is_decreasing = is_decreasing_sequence(levels)

    if not is_increasing and not is_decreasing:
        return False

    # Check that all adjacent levels differ by at least 1 and at most 3
    for i in range(len(levels) - 1):
        diff = abs(levels[i + 1] - levels[i])
        if diff < 1 or diff > 3:
            return False

    return True


def is_increasing_sequence(numbers):
    for i in range(1, len(numbers)):
        if numbers[i] < numbers[i - 1]:
            return False
    return True


def is_decreasing_sequence(numbers):
    for i in range(1, len(numbers)):
        if numbers[i] > numbers[i - 1]:
            return False
    return True


if __name__ == "__main__":
    main()
