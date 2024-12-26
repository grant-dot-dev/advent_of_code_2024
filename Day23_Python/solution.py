from itertools import combinations

connections = {}
triangles = set()


# Read the input file and process connections
def parse_input():
    with open('input.txt', 'r') as file:
        for line in file:
            connection = line.strip().split('-')
            if len(connection) == 2:
                node1, node2 = connection

                # Add the connection in both directions
                if node1 not in connections:
                    connections[node1] = set()
                if node2 not in connections:
                    connections[node2] = set()

                connections[node1].add(node2)
                connections[node2].add(node1)


# Find sets of three computers where each is connected to the other two
def find_connections():
    for node in connections:
        # Check all pairs of neighbors for each node
        neighbors = connections[node]
        for neighbor1, neighbor2 in combinations(neighbors, 2):
            # Check if the neighbors are connected to each other
            if neighbor2 in connections[neighbor1]:
                # Sort and store the triangle to avoid duplicates
                triangle = tuple(sorted([node, neighbor1, neighbor2]))
                triangles.add(triangle)


def find_largest_clique_bron_kerbosch():
    def bron_kerbosch(r, p, x):
        if not p and not x:
            cliques.append(r)
            return

        for node in list(p):
            bron_kerbosch(
                r | {node},                      # Add node to clique
                p & connections[node],           # Neighbors of node in p
                x & connections[node]            # Neighbors of node in x
            )
            p.remove(node)  # Remove node from p
            x.add(node)     # Add node to x (processed)

    adj_list = {k: set(v) for k, v in connections.items()}
    cliques = []
    bron_kerbosch(set(), set(adj_list.keys()), set())
    return max(cliques, key=len)  # Return the largest clique


if __name__ == '__main__':
    parse_input()
    find_connections()

    triangles_with_t = [triangle for triangle in triangles if any(
        node.startswith('t') for node in triangle)]

    print(len(triangles_with_t))
    largest_set = find_largest_clique_bron_kerbosch()

    sorted_set = sorted(largest_set)
    print(sorted_set)
