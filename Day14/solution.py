from re import findall
from math import prod
from operator import itemgetter

# Determine if we are running the example or full input
is_local = False


with open("example.txt" if is_local else "robots.txt", "r") as file:
    robots = file.readlines()


width = 11 if is_local else 101
height = 7 if is_local else 103

middle_row_gap = width // 2
middle_column_gap = height // 2

# Parse robot data
robot_data = []
for robot in robots:
    p_x, p_y, v_x, v_y = map(int, findall(r"-?\d+", robot))
    robot_data.append((p_x, p_y, v_x, v_y))


# Function to simulate robot positions at time t
def simulate(robots, t):
    return [((p_x + t * v_x) % width, (p_y + t * v_y) % height) for p_x, p_y, v_x, v_y in robots]


# ==== PART 1 ====
# Simulate robots at t=100
simulation = simulate(robot_data, t=100)

# Initialize quadrant counts
quadrants = [0, 0, 0, 0]

# Count robots in each quadrant
for x, y in simulation:
    if x < middle_row_gap and y < middle_column_gap:
        quadrants[0] += 1
    elif x >= middle_row_gap and y < middle_column_gap:
        quadrants[1] += 1
    elif x >= middle_row_gap and y >= middle_column_gap:
        quadrants[2] += 1
    elif x < middle_row_gap and y >= middle_column_gap:
        quadrants[3] += 1


print("Part 1:", prod(quadrants))


# ==== PART 2 ====

# Define quadrants for the Christmas tree region detection
tree_time = 0
T = range(width * height)  # Maximum time steps to simulate
simulations = [simulate(robot_data, t) for t in T]

# Christmas tree detection: progressively shrinking bounding box
for i in range(1, min(width, height)):
    # Define target region
    a, b, c, d = 0 + i, width - 1 - i, 0 + i, height - 1 - i

    # Calculate the target region area
    area = (b - a + 1) * (d - c + 1)

    # Calculate densities for each time step
    densities = [
        sum(a <= x <= b and c <= y <= d for x, y in sim) / area for sim in simulations
    ]

    # Find the time step with the highest density
    t, max_density = max(zip(T, densities), key=itemgetter(1))

    # Break if the tree region doesn't change
    if t == tree_time:
        break

    tree_time = t

# Output time when Christmas tree forms
print("Part 2:", tree_time)
