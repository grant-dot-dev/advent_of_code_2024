import time


def load_input(file_path):
    with open(file_path, 'r') as file:
        lines = file.read().splitlines()

    towels = lines[0].split(", ")

    arrangements = [line for line in lines[2:]
                    if line.strip()]  # Skip blank lines

    return towels, arrangements


def can_construct(design, towels, memo):
    if design in memo:
        return memo[design]

    if design == "":
        return True

    for towel in towels:
        if design.startswith(towel):  # Check if towel matches the start of design
            remainder = design[len(towel):]
            if can_construct(remainder, towels, memo):
                memo[design] = True
                return True

    memo[design] = False
    return False


def count_arrangements(design, towels, memo_count):

    if design in memo_count:
        return memo_count[design]

    if design == "":
        return 1

    total_ways = 0
    for towel in towels:
        if design.startswith(towel):  # Check if towel matches the start of design
            remainder = design[len(towel):]
            total_ways += count_arrangements(remainder, towels, memo_count)

    memo_count[design] = total_ways  # Cache the result
    return total_ways


def main():
    is_development = True
    file_path = "input.txt" if is_development == False else "example.txt"
    towels, arrangements = load_input(file_path)

    # Part 1: Check if designs are possible
    memo = {}
    possible_count = 0

    for arrangement in arrangements:
        if can_construct(arrangement, towels, memo):
            possible_count += 1

    print(f"Part 1 result: {possible_count}")

    # Part 2: Count all possible arrangements
    memo_count = {}
    total_arrangements = 0

    for arrangement in arrangements:
        if arrangement in memo and memo[arrangement]:
            ways = count_arrangements(arrangement, towels, memo_count)
            total_arrangements += ways

    print(f"Part 2 result: {total_arrangements}")


if __name__ == "__main__":
    start_time = time.time()
    main()
    print("--- %s seconds ---" % (time.time() - start_time))
