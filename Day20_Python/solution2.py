import numpy as np


def read_grid(filename):
    with open(filename) as f:
        return np.array([list(line) for line in f.read().splitlines()])


def find_position(grid, target):
    positions = [(i, j) for i, line in enumerate(grid)
                 for j, char in enumerate(line) if char == target]
    assert len(positions) == 1  # Ensure only one position is found
    return positions[0]


def get_valid_neighbors(grid, position, visited):
    i, j = position
    # Directions: up, left, right, down
    directions = [(-1, 0), (0, -1), (0, 1), (1, 0)]
    valid_neighbors = []
    for di, dj in directions:
        newi, newj = i + di, j + dj
        if 0 <= newi < grid.shape[0] and 0 <= newj < grid.shape[1] and (newi, newj) not in visited:
            # Valid move condition (can be 'S', 'E', or '.')
            if grid[newi][newj] in 'SE.':
                valid_neighbors.append((newi, newj))
    return valid_neighbors


def traverse_grid(grid, start, end):
    visited = {start: 0}
    current_position = start
    current_step = 0

    while current_position != end:
        current_step += 1
        neighbors = get_valid_neighbors(grid, current_position, visited)
        for neighbor in neighbors:
            visited[neighbor] = current_step
            current_position = neighbor
            break  # Move to the first valid neighbor
    return visited


def get_cheat_endpoints(coords, track):
    """Finds nearby positions (within a 20 unit range) that are in the track."""
    i, j = coords
    nearby_positions = set()
    for di in range(-20, 21):
        dj_max = 20 - abs(di)
        for dj in range(-dj_max, dj_max + 1):
            neighbor = (i + di, j + dj)
            if neighbor in track:
                nearby_positions.add(neighbor)
    return nearby_positions


def manhattan_distance(coord1, coord2):
    """Calculates the Manhattan distance between two coordinates."""
    return sum(abs(i - j) for i, j in zip(coord1, coord2))


def main():
    grid = read_grid('input.txt')
    start = find_position(grid, 'S')
    end = find_position(grid, 'E')

    # Traverse the grid and record steps
    track = traverse_grid(grid, start, end)

    # Count positions meeting the condition
    count = 0
    for coords in track:
        nearby_positions = get_cheat_endpoints(coords, track)
        for other_coords in nearby_positions:
            distance = manhattan_distance(coords, other_coords)
            if track[other_coords] - track[coords] - distance >= 100:
                count += 1

    print(count)


if __name__ == "__main__":
    main()
