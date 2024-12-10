import time
from pathlib import Path

start_time = time.time()
input_lines = open("input.txt", "r").read().splitlines()
disk_map = [int(value) for value in input_lines[0]]

EMPTY_SLOT = -1
block_distribution = []
current_block_id = -1

for index, count in enumerate(disk_map):
    if index % 2 == 0:
        current_block_id += 1
    block_distribution.append({
        "id": current_block_id if index % 2 == 0 else EMPTY_SLOT,
        "count": count
    })


def calculate_checksum(blocks: list):
    total_checksum = 0
    position_counter = 0
    for block in blocks:
        for _ in range(block["count"]):
            if block["id"] != EMPTY_SLOT:
                total_checksum += block["id"] * position_counter
            position_counter += 1
    return total_checksum


pickup_pointer = len(block_distribution) - 1
start_index_cache = {}

while True:
    while block_distribution[pickup_pointer]["id"] == EMPTY_SLOT and pickup_pointer > 0:
        pickup_pointer -= 1

    if pickup_pointer == 0:
        break

    required_space = block_distribution[pickup_pointer]["count"]
    finder_pointer = start_index_cache[required_space] if required_space in start_index_cache else 0

    while (block_distribution[finder_pointer]["id"] != EMPTY_SLOT or
           block_distribution[finder_pointer]["count"] < required_space) and finder_pointer < pickup_pointer:
        finder_pointer += 1

    start_index_cache[required_space] = finder_pointer

    if finder_pointer >= pickup_pointer:
        pickup_pointer -= 1
        continue

    block_to_move = block_distribution.pop(pickup_pointer)
    block_distribution.insert(
        pickup_pointer, {"id": EMPTY_SLOT, "count": block_to_move["count"]})
    pickup_pointer -= 1

    block_distribution[finder_pointer]["count"] -= required_space
    if block_distribution[finder_pointer]["count"] == 0:
        block_distribution[finder_pointer] = block_to_move
    else:
        block_distribution.insert(finder_pointer, block_to_move)
        pickup_pointer += 1

print("--- %s seconds ---" % (time.time() - start_time))
print("result", calculate_checksum(block_distribution))
