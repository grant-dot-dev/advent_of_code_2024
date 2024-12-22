from collections import defaultdict
from itertools import permutations
from dataclasses import dataclass
from typing import Dict, List, Tuple


@dataclass(frozen=True)
class Location:
    x: int
    y: int

    def delta(self, other: 'Location') -> Tuple[int, int]:
        return other.x - self.x, other.y - self.y

    def __add__(self, other: 'Location') -> 'Location':
        return Location(self.x + other.x, self.y + other.y)


# Define the keypad layouts and movement directions
NUM_PAD = {
    '7': Location(0, 0), '8': Location(1, 0), '9': Location(2, 0),
    '4': Location(0, 1), '5': Location(1, 1), '6': Location(2, 1),
    '1': Location(0, 2), '2': Location(1, 2), '3': Location(2, 2),
    '0': Location(1, 3), 'A': Location(2, 3),
}

KEY_PAD = {
    '^': Location(1, 0), 'a': Location(2, 0),
    '<': Location(0, 1), 'v': Location(1, 1),
    '>': Location(2, 1),
}

DIRECTIONS = {
    '^': Location(0, -1), 'v': Location(0, 1),
    '<': Location(-1, 0), '>': Location(1, 0),
}

cache: Dict[Tuple[str, int, int], int] = {}
moves_cache: Dict[Tuple[str, str], List[str]] = {}


def shortest_length(code: str, depth_limit: int, cur_depth: int) -> int:
    if (code, depth_limit, cur_depth) in cache:
        return cache[(code, depth_limit, cur_depth)]

    current_char = 'A' if cur_depth == 0 else 'a'
    total_length = 0

    for target_char in code:
        if cur_depth == depth_limit:
            total_length += len(moves_cache[(current_char, target_char)][0])
        else:
            total_length += min(
                shortest_length(remaining_code, depth_limit, cur_depth + 1)
                for remaining_code in moves_cache[(current_char, target_char)]
            )
        current_char = target_char

    cache[(code, depth_limit, cur_depth)] = total_length
    return total_length


def moves_between_positions(start: Location, end: Location, is_keypad: bool = True) -> List[str]:
    if start == end:
        return ["a"]

    delta_x, delta_y = start.delta(end)
    steps = []
    if delta_x < 0:
        steps.append('<' * abs(delta_x))
    else:
        steps.append('>' * abs(delta_x))
    if delta_y < 0:
        steps.append('^' * abs(delta_y))
    else:
        steps.append('v' * abs(delta_y))

    valid_moves = []
    for perm in permutations(''.join(steps)):
        current_location = start
        valid = True
        for move in perm:
            current_location += DIRECTIONS[move]
            if (is_keypad and current_location == Location(0, 0)) or (
                not is_keypad and current_location == Location(0, 3)
            ):
                valid = False
                break
        if valid:
            valid_moves.append(''.join(perm) + 'a')

    return list(set(valid_moves))


def create_cache_moves():
    for (key1, loc1), (key2, loc2) in permutations(NUM_PAD.items(), 2):
        moves_cache[(key1, key2)] = moves_between_positions(
            loc1, loc2, is_keypad=False)

    for key in NUM_PAD.keys():
        moves_cache[(key, key)] = moves_between_positions(
            NUM_PAD[key], NUM_PAD[key], is_keypad=False)

    for (key1, loc1), (key2, loc2) in permutations(KEY_PAD.items(), 2):
        moves_cache[(key1, key2)] = moves_between_positions(loc1, loc2)

    for key in KEY_PAD.keys():
        moves_cache[(key, key)] = moves_between_positions(
            KEY_PAD[key], KEY_PAD[key])


def solve_part_one(input_data: str) -> int:
    total = 0
    for code in input_data.splitlines():
        value = int(code[:3])
        total += shortest_length(code, 2, 0) * value
    return total


def solve_part_two(input_data: str) -> int:
    total = 0
    for code in input_data.splitlines():
        value = int(code[:3])
        total += shortest_length(code, 25, 0) * value
    return total


create_cache_moves()

# Example usage
if __name__ == "__main__":
    input = """805A\n170A\n129A\n283A\n379A"""
    print("Part One:", solve_part_one(input))
    print("Part Two:", solve_part_two(input))
