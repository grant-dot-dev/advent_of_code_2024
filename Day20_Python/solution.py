def parse_grid(filename):
    with open(filename) as f:
        return f.read().splitlines()


def find_position(grid, target):
    for i, row in enumerate(grid):
        for j, char in enumerate(row):
            if char == target:
                return (i, j)


def is_within_bounds(grid, position):
    i, j = position
    return 0 <= i < len(grid) and 0 <= j < len(grid[0])


def can_move(grid, position, visited):
    i, j = position
    return is_within_bounds(grid, position) and position not in visited and grid[i][j] in 'SE.'


def traverse_grid(grid, start, end):
    visited = {start: 0}
    current_position = start
    current_step = 0

    while current_position != end:
        current_step += 1
        i, j = current_position
        for di, dj in [(-1, 0), (0, -1), (0, 1), (1, 0)]:  # Directions: up, left, right, down
            new_position = (i + di, j + dj)
            if can_move(grid, new_position, visited):
                current_position = new_position
                visited[current_position] = current_step
                break

    return visited


def count_special_positions(grid, visited):
    count = 0
    for i, j in visited:
        for di, dj in [(-1, 0), (0, -1), (0, 1), (1, 0)]:
            first_neighbor = (i + di, j + dj)
            second_neighbor = (i + 2 * di, j + 2 * dj)
            if (first_neighbor not in visited and
                second_neighbor in visited and
                    visited[second_neighbor] - visited[(i, j)] >= 102):
                count += 1
    return count


def main():
    # Parse the grid and locate start ('S') and end ('E') positions
    grid = parse_grid('input.txt')
    start = find_position(grid, 'S')
    end = find_position(grid, 'E')

    # Traverse the grid and count positions
    visited = traverse_grid(grid, start, end)
    result = count_special_positions(grid, visited)

    print(result)


if __name__ == "__main__":
    main()
