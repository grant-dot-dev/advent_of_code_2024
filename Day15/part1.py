import os
from collections import deque

INPUT_FILE_PATH = os.path.join(os.path.dirname(__file__), 'input.txt')


def parse_grid(data) -> tuple:
    grid, instructions = {}, deque()
    lines = data
    start_position = (-1, -1)

    for i, line in enumerate(lines):
        if line == "":
            instructions.extend(list(''.join(lines[i+1:])))
            break

        for j, cell in enumerate(line):
            grid[(i, j)] = cell
            if cell == "@":
                start_position = (i, j)

    return grid, instructions, start_position


def modify_grid(data) -> tuple:
    lines = data.splitlines()
    for i, line in enumerate(lines):
        lines[i] = line.replace("#", "##").replace(
            "O", "[]").replace(".", "..").replace("@", "@.")
    return parse_grid(lines)


def find_boxes(grid) -> dict:
    boxes = {}
    for position, value in grid.items():
        if value == '[':
            boxes[position] = (position[0], position[1] + 1)
        elif value == ']':
            boxes[position] = (position[0], position[1] - 1)
    return boxes


def move_robot(grid, instructions, start_position):
    current_position = start_position

    while instructions:
        move = instructions.popleft()

        direction = {'^': (-1, 0), '>': (0, 1), 'v': (1, 0), '<': (0, -1)}
        if move not in direction:
            raise ValueError(f"Unknown movement: {move}")

        dx, dy = direction[move]
        next_position = (current_position[0] + dx, current_position[1] + dy)

        if grid.get(next_position) == '#':
            continue

        if grid.get(next_position) == '.':
            grid[current_position] = '.'
            grid[next_position] = '@'
        elif grid.get(next_position) == 'O':
            next_item_position = (next_position[0] + dx, next_position[1] + dy)
            if grid.get(next_item_position) == '.':
                grid[next_item_position] = 'O'
                grid[next_position] = '@'
            elif grid.get(next_item_position) == 'O':
                pos = current_position
                shifts, space_available = 0, False

                while grid.get(pos) != '#':
                    if grid.get(pos) == '.':
                        space_available = True
                        break
                    pos = (pos[0] + dx, pos[1] + dy)
                    shifts += 1

                if not space_available:
                    continue

                pos = (pos[0] - dx, pos[1] - dy)

                for _ in range(shifts):
                    val = grid.get(pos)
                    grid[(pos[0] + dx, pos[1] + dy)] = val
                    grid[pos] = '.'
                    pos = (pos[0] - dx, pos[1] - dy)
            else:
                continue

        grid[current_position] = '.'
        current_position = next_position


def move_robot_in_second_warehouse(grid, instructions, start_position):
    current_position = start_position

    while instructions:
        move = instructions.popleft()

        direction = {'^': (-1, 0), '>': (0, 1), 'v': (1, 0), '<': (0, -1)}
        dx, dy = direction.get(move)

        next_position = (current_position[0] + dx, current_position[1] + dy)

        if grid.get(next_position) == '#':
            continue

        if grid.get(next_position) == '.':
            grid[current_position] = '.'
            grid[next_position] = '@'
        elif grid.get(next_position) in ('[', ']'):
            boxes = find_boxes(grid)
            top_points = set()
            to_move = set()
            shifts, space_available = 0, False

            positions = [next_position]
            if grid[next_position] in ('[', ']') and move in ('^', 'v'):
                positions.append(boxes.get(next_position))

            to_move.add(next_position)
            to_move.add(boxes.get(next_position))

            while positions:
                shifts = 0
                pp = positions.pop()
                while True:
                    if grid.get(pp) == '.':
                        if move in ('<', '>'):
                            space_available = True
                            top_points.add((pp[0] - dx, pp[1] - dy))
                            to_move.clear()
                            positions.clear()
                            break

                        second_half = boxes.get((pp[0] - dx, pp[1] - dy))

                        if grid.get((second_half[0] + dx, second_half[1] + dy)) == '.':
                            space_available = True
                            top_points.add((pp[0] - dx, pp[1] - dy))
                            break
                        if grid.get((second_half[0] + dx, second_half[1] + dy)) == '#':
                            space_available = False
                            break
                        if grid.get((second_half[0] + dx, second_half[1] + dy)) in ('[', ']'):
                            pp = (second_half[0] + dx, second_half[1] + dy)
                            continue
                    elif grid.get(pp) == '#':
                        space_available = False
                        break
                    elif grid.get(pp) in ('[', ']'):
                        if boxes.get(pp) not in to_move:
                            positions.append(boxes.get(pp))
                            to_move.add(boxes.get(pp))
                        to_move.add(pp)

                    pp = (pp[0] + dx, pp[1] + dy)
                    shifts += 1

                if not space_available:
                    break

            if not space_available:
                continue

            if to_move:
                halves = [boxes.get(p) for p in to_move]
                to_move.update(halves)

                to_move = sorted(to_move, key=lambda x: (x[1], x[0]))
                if move == 'v':
                    to_move.reverse()

                for p in to_move:
                    grid[(p[0] + dx, p[1])] = grid.get(p)
                    grid[p] = '.'
            else:
                for p in top_points:
                    for _ in range(shifts):
                        val = grid.get(p)
                        grid[(p[0] + dx, p[1] + dy)] = val
                        grid[p] = '.'
                        p = (p[0] - dx, p[1] - dy)
        else:
            continue

        grid[current_position] = '.'
        grid[next_position] = '@'
        current_position = next_position


def part_one(data) -> int:
    grid, instructions, start_position = parse_grid(data.splitlines())
    move_robot(grid, instructions, start_position)

    # add up all the boxes
    return sum(100 * x + y for (x, y), value in grid.items() if value == 'O')


def part_two(data) -> int:
    grid, instructions, start_position = modify_grid(data)
    move_robot_in_second_warehouse(grid, instructions, start_position)

    # add up all the boxes in the second warehouse
    return sum(100 * x + y for (x, y), value in grid.items() if value == '[')


def main() -> int:
    with open(INPUT_FILE_PATH) as file:
        data = file.read()
        print("Part 1:", part_one(data))
        print("Part 2:", part_two(data))
    return 0


if __name__ == '__main__':
    raise SystemExit(main())
