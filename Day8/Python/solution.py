from itertools import combinations
from math import gcd
from collections import defaultdict


def parse_grid(lines):
    grid_points = set()
    for y, line in enumerate(lines):
        for x, char in enumerate(line):
            grid_points.add((x, y))
    return grid_points


def build_antenna_locations(grid, lines):
    antennas = defaultdict(list)
    for y, line in enumerate(lines):
        for x, char in enumerate(line):
            if char != '.':
                antennas[char].append((x, y))
    return antennas


def part1(grid, antenna_locations):
    antinodes = set()

    for coords in antenna_locations.values():
        for (start_y, start_x), (end_y, end_x) in combinations(coords, 2):

            dy = start_y - end_y
            dx = start_x - end_x

            aa = (start_x + dx, start_y + dy)
            bb = (end_x - dx, end_y - dy)

            if aa in grid:
                antinodes.add(aa)
            if bb in grid:
                antinodes.add(bb)

    return len(antinodes)


def part2(grid, antenna_locations):
    antinodes = set()

    for coords in antenna_locations.values():
        for (start_y, start_x), (end_y, end_x) in combinations(coords, 2):

            dy = start_y - end_y
            dx = start_x - end_x
            g = gcd(dy, dx)
            dy //= g
            dx //= g

            # Iterate over the points along the direction of the vector in both directions
            i = 0
            while True:
                aa = (start_x + dx * i, start_y + dy * i)
                if aa in grid:
                    antinodes.add(aa)
                    i += 1
                else:
                    break

            i = 0
            while True:
                bb = (end_x - dx * i, end_y - dy * i)
                if bb in grid:
                    antinodes.add(bb)
                    i += 1
                else:
                    break

    return len(antinodes)


def main():
    with open("./test.txt", "r") as file:
        lines = file.read().strip().splitlines()

    grid = parse_grid(lines)
    antenna_locations = build_antenna_locations(grid, lines)

    part1_result = part1(grid, antenna_locations)
    part2_result = part2(grid, antenna_locations)

    print(f"Part 1: {part1_result}")
    print(f"Part 2: {part2_result}")


if __name__ == "__main__":
    main()
