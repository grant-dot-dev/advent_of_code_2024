def load_grid_from_file(file_path):
    # Read file contents
    with open(file_path, 'r') as file:
        lines = file.read().splitlines()

    # Convert to a grid dictionary
    grid = {}
    for y, line in enumerate(lines):
        for x, char in enumerate(line):
            grid[(x, y)] = char

    return grid
