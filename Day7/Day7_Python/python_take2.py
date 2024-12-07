import argparse
import re
from itertools import product

# Function to add two numbers


def add(a: int, b: int) -> int:
    return a + b

# Function to multiply two numbers


def multiply(a: int, b: int) -> int:
    return a * b

# Function to concatenate two numbers


def concatenate(a: int, b: int) -> int:
    return int(f"{a}{b}")

# Function to compute the equation


def compute_equation(line: str, operations: list) -> bool:
    nums = list(map(int, re.findall(r"\d+", line)))
    target, nums = nums[0], nums[1:]
    score = None
    for combination in list(product(operations, repeat=len(nums) - 1)):
        for i, operation in enumerate(combination):
            if i == 0:
                score = operation(nums[i], nums[i + 1])
            else:
                score = operation(score, nums[i + 1])
        if score == target:
            return target
    return 0

# Function for part 1


def part1(lines):
    final_score = 0
    for line in lines:
        final_score += compute_equation(line, operations=[add, multiply])
    return final_score

# Function for part 2


def part2(lines):
    final_score = 0
    for line in lines:
        final_score += compute_equation(line,
                                        operations=[add, multiply, concatenate])
    return final_score


# Load the input file and process it
with open('./input1.txt', 'r') as file:
    lines = file.readlines()

part1_score = part1(lines)
print(f"Part 1 Score: {part1_score}")

part2_score = part2(lines)
print(f"Part 2 Score: {part2_score}")
