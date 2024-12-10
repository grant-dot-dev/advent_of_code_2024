import time
from pathlib import Path

start_time = time.time()
input_lines = open("./input.txt", "r").read().splitlines()
disk_map = [int(value) for value in input_lines[0]]

EMPTY_SLOT = -1
block_distribution = []
block_id = 0

for index, count in enumerate(disk_map):
    if index % 2 == 0:
        block_distribution += [block_id for _ in range(count)]
        block_id += 1
    else:
        block_distribution += [EMPTY_SLOT for _ in range(count)]


def calculate_checksum(blocks: list):
    return sum([value * position if value != EMPTY_SLOT else 0 for position, value in enumerate(blocks)])


placement_pointer = 0
pickup_pointer = len(block_distribution) - 1

while placement_pointer < pickup_pointer:
    while block_distribution[placement_pointer] != EMPTY_SLOT and placement_pointer < pickup_pointer:
        placement_pointer += 1
    while block_distribution[pickup_pointer] == EMPTY_SLOT and pickup_pointer > placement_pointer:
        pickup_pointer -= 1
    if placement_pointer == pickup_pointer:
        break

    block_to_move = block_distribution.pop(pickup_pointer)
    pickup_pointer -= 1
    block_distribution[placement_pointer] = block_to_move


print("--- %s seconds ---" % (time.time() - start_time))
print("result", calculate_checksum(block_distribution))
