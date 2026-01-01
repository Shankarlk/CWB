var menusdata = {};
var menusdataEmpl = {};
var Departments = {};
let isFormDirty = false;
let isInitializing = false;

//async function loadFlowchart() {
//    const response = await fetch('/Employee/GetFlowchartList');
//    const data = await response.json();
//    const mermaidSyntax = "graph TD\n" + buildMermaidGraph(data, "CWB");

//    const flowchartContainer = document.getElementById('flowchart');
//    flowchartContainer.innerHTML = `<div class="mermaid">${mermaidSyntax}</div>`;

//    // Initialize Mermaid and wait for it to render
//    await mermaid.init(undefined, flowchartContainer.querySelector('.mermaid'));

//    // Find the rendered SVG element
//    const svgElement = flowchartContainer.querySelector('svg');

//    if (svgElement) {
//        const panZoomInstance = svgPanZoom(svgElement, {
//            zoomEnabled: true,
//            panEnabled: true,
//            dblClickZoomEnabled: false,
//            controlIconsEnabled: true,
//            fit: false,  // Set to false to manually control the zoom
//            center: false, // Set to false to manually control the center
//            minZoom: 0.1,
//            maxZoom: 10
//        });

//        // Set the initial zoom level to fit the entire graph within the view.
//        // This is often more reliable than the 'fit: true' option.
//        panZoomInstance.fit();
//        panZoomInstance.zoomAtPoint(1, { x: 0, y: 0 }); // Zoom level 1
//        panZoomInstance.center();
//    } else {
//        console.error("Mermaid SVG not found.");
//    }
//}

//function buildMermaidGraph(tree, parent) {
//    let result = '';

//    tree.forEach(node => {
//        // Sanitize ID (remove spaces and special characters)
//        const nodeId = node.uI_Name_Label.replace(/[^a-zA-Z0-9]/g, "_");
//        const parentId = parent.replace(/[^a-zA-Z0-9]/g, "_");

//        // Create connection: ID["Label"]
//        result += `${parentId}["${parent}"] --> ${nodeId}["${node.uI_Name_Label}"]\n`;

//        // Recursively process children
//        if (node.children && node.children.length > 0) {
//            result += buildMermaidGraph(node.children, node.uI_Name_Label);
//        }
//    });

//    return result;
//}


//// Call on page load
//window.onload = loadFlowchart;
//document.addEventListener("DOMContentLoaded", function () {
//    fetch('/Employee/GetAllUIlist')
//        .then(res => res.json())
//        .then(data => {
//            const nodes = [];
//            const edges = [];

//            // Root node
//            nodes.push({
//                id: 0,
//                label: "CWB",
//                shape: "box",
//                color: "#3f51b5",
//                font: { color: "#fff", size: 20 }
//            });

//            data.forEach(item => {
//                nodes.push({
//                    id: item.uiListId,
//                    label: item.uI_Name_Label,
//                    shape: "box",   // ✅ Rectangular
//                    font: { size: 16 }
//                });

//                if (item.uI_Part_linked_to && item.uI_Part_linked_to !== 0) {
//                    edges.push({ from: item.uI_Part_linked_to, to: item.uiListId });
//                }

//                if (item.topLevelId === "Y") {
//                    edges.push({ from: 0, to: item.uiListId });
//                }
//            });

//            const container = document.getElementById('flowchart');
//            const networkData = {
//                nodes: new vis.DataSet(nodes),
//                edges: new vis.DataSet(edges)
//            };

//            const options = {
//                layout: {
//                    hierarchical: {
//                        direction: "UD",
//                        sortMethod: "directed",
//                        nodeSpacing: 200,
//                        levelSeparation: 200,
//                        treeSpacing: 250
//                    }
//                },
//                physics: false,
//                interaction: {
//                    dragNodes: true,
//                    dragView: true,  // ✅ allow scroll/drag
//                    zoomView: true   // ✅ allow zoom
//                },
//                nodes: {
//                    shape: "box",
//                    margin: 10,
//                    font: {
//                        size: 18,
//                        face: "arial",
//                        color: "#111"
//                    },
//                    borderWidth: 2,
//                    color: {
//                        background: "#e3f2fd",
//                        border: "#1565c0",
//                        highlight: { background: "#bbdefb", border: "#0d47a1" }
//                    }
//                },
//                edges: {
//                    arrows: "to",
//                    smooth: false,
//                    color: { color: "#666", highlight: "#000" }
//                }
//            };

//            const network = new vis.Network(container, networkData, options);

//            // ✅ Fit but not too small: set min zoom
//            network.once("stabilizationIterationsDone", function () {
//                network.fit({
//                    animation: { duration: 1000, easingFunction: "easeInOutQuad" },
//                    minZoomLevel: 0.8, // prevent zooming out too far
//                    maxZoomLevel: 2.5  // prevent too close
//                });
//            });
//        });
//});

/// -- original 
//document.addEventListener("DOMContentLoaded", function () {
//    fetch('/Employee/GetAllUIlist')
//        .then(res => res.json())
//        .then(data => {
//            const nodes = [];
//            const edges = [];

//            // Root node
//            nodes.push({
//                id: 0,
//                label: "CWB",
//                shape: "box",
//                color: "#3f51b5",
//                font: { color: "#fff", size: 20 }
//            });

//            data.forEach(item => {
//                nodes.push({
//                    id: item.uiListId,
//                    label: item.uI_Name_Label,
//                    shape: "box",   // ✅ rectangle
//                    font: { size: 16 }
//                });

//                if (item.uI_Part_linked_to && item.uI_Part_linked_to !== 0) {
//                    edges.push({ from: item.uI_Part_linked_to, to: item.uiListId });
//                }

//                if (item.topLevelId === "Y") {
//                    edges.push({ from: 0, to: item.uiListId });
//                }
//            });

//            const container = document.getElementById('flowchart');
//            const networkData = {
//                nodes: new vis.DataSet(nodes),
//                edges: new vis.DataSet(edges)
//            };

//            const options = {
//                layout: {
//                    hierarchical: {
//                        direction: "UD",
//                        sortMethod: "directed",
//                        levelSeparation: 250, // vertical distance
//                        nodeSpacing: 200,     // horizontal distance
//                        treeSpacing: 300
//                    }
//                },
//                physics: {
//                    enabled: true,
//                    barnesHut: {
//                        gravitationalConstant: -5000, // push nodes apart
//                        springLength: 300,            // edge length
//                        springConstant: 0.02
//                    }
//                },
//                interaction: {
//                    dragNodes: true,
//                    dragView: true,   // ✅ allow scrolling
//                    zoomView: true    // ✅ allow zooming
//                },
//                nodes: {
//                    shape: "box",
//                    margin: 10,
//                    font: {
//                        size: 18,
//                        face: "arial",
//                        color: "#111"
//                    },
//                    borderWidth: 2,
//                    color: {
//                        background: "#e3f2fd",
//                        border: "#1565c0",
//                        highlight: { background: "#bbdefb", border: "#0d47a1" }
//                    }
//                },
//                edges: {
//                    arrows: "to",
//                    smooth: false,
//                    color: { color: "#666", highlight: "#000" }
//                }
//            };

//            const network = new vis.Network(container, networkData, options);

//            // ✅ Instead of fitting entire graph → focus on root node (CWB)
//            network.once("stabilizationIterationsDone", function () {
//                network.focus(0, {   // Focus on node ID = 0 (CWB)
//                    scale: 1.2,      // Zoom level (adjust as needed)
//                    animation: { duration: 1000, easingFunction: "easeOutCubic" }
//                });
//            });
//            window.addEventListener("resize", function () {
//                network.fit();
//            });
//        });
//});

//document.addEventListener("DOMContentLoaded", function () {
//    fetch('/Employee/GetAllUIlist')
//        .then(res => res.json())
//        .then(data => {
//            const nodes = [];
//            const edges = [];

//            // Root node
//            nodes.push({
//                id: 0,
//                label: "CWB",
//                shape: "box",
//                color: "#3f51b5",
//                font: { color: "#fff", size: 20 }
//            });

//            data.forEach(item => {
//                nodes.push({
//                    id: item.uiListId,
//                    label: item.uI_Name_Label,
//                    shape: "box",
//                    font: { size: 16 }
//                });

//                if (item.uI_Part_linked_to && item.uI_Part_linked_to !== 0) {
//                    edges.push({ from: item.uI_Part_linked_to, to: item.uiListId });
//                }

//                if (item.topLevelId === "Y") {
//                    edges.push({ from: 0, to: item.uiListId });
//                }
//            });

//            const container = document.getElementById('flowchart');
//            const networkData = {
//                nodes: new vis.DataSet(nodes),
//                edges: new vis.DataSet(edges)
//            };

//            const options = {
//                layout: {
//                    hierarchical: {
//                        direction: "UD",
//                        sortMethod: "directed",
//                        levelSeparation: 250,
//                        nodeSpacing: 200,
//                        treeSpacing: 300
//                    }
//                },
//                physics: {
//                    enabled: true,
//                    barnesHut: {
//                        gravitationalConstant: -5000,
//                        springLength: 300,
//                        springConstant: 0.02
//                    }
//                },
//                interaction: {
//                    dragNodes: true,
//                    dragView: true,
//                    zoomView: true
//                },
//                nodes: {
//                    shape: "box",
//                    margin: 10,
//                    font: {
//                        size: 18,
//                        face: "arial",
//                        color: "#111"
//                    },
//                    borderWidth: 2,
//                    color: {
//                        background: "#e3f2fd",
//                        border: "#1565c0",
//                        highlight: { background: "#bbdefb", border: "#0d47a1" }
//                    }
//                },
//                edges: {
//                    arrows: "to",
//                    smooth: false,
//                    color: { color: "#666", highlight: "#000" }
//                }
//            };

//            const network = new vis.Network(container, networkData, options);

//            // This is the correct logic for your goal
//            network.once("stabilizationIterationsDone", function () {
//                network.fit();
//            });
//        });
//});

//Cytoscape
//document.addEventListener("DOMContentLoaded", function () {
//    fetch('/Employee/GetAllUIlist')
//        .then(res => res.json())
//        .then(data => {
//            const elements = [];

//            // Root node (CWB)
//            elements.push({
//                data: { id: "0", label: "CWB" },
//                classes: "root"
//            });

//            data.forEach(item => {
//                // Add node
//                elements.push({
//                    data: { id: String(item.uiListId), label: item.uI_Name_Label }
//                });

//                // Add edge if linked
//                if (item.uI_Part_linked_to && item.uI_Part_linked_to !== 0) {
//                    elements.push({
//                        data: { source: String(item.uI_Part_linked_to), target: String(item.uiListId) }
//                    });
//                }

//                // Ensure "Info-Queries" is a child of "CWB"
//                if (item.uI_Name_Label === "Info-Queries") {
//                    elements.push({
//                        data: { source: "0", target: String(item.uiListId) }
//                    });
//                }

//                // Link top level items to root
//                if (item.topLevelId === "Y") {
//                    elements.push({
//                        data: { source: "0", target: String(item.uiListId) }
//                    });
//                }
//            });

//            // Initialize Cytoscape
//            const cy = cytoscape({
//                container: document.getElementById('flowchart'),
//                elements: elements,
//                style: [
//                    {
//                        selector: 'node',
//                        style: {
//                            'background-color': '#e3f2fd',
//                            'label': 'data(label)',
//                            'color': '#111',
//                            'text-valign': 'center',
//                            'text-halign': 'center',
//                            'font-size': 14,
//                            'border-width': 2,
//                            'border-color': '#1565c0',
//                            'shape': 'round-rectangle',
//                            'padding': '10px',
//                            'text-wrap': 'wrap',
//                            'text-max-width': '100px',
//                            'width': 'label',
//                            'height': 'label',
//                        }
//                    },
//                    {
//                        selector: 'node.root',
//                        style: {
//                            'background-color': '#3f51b5',
//                            'color': '#fff',
//                            'font-size': 18,
//                            'border-color': '#0d47a1',
//                            'width': '100px',
//                            'height': '40px'
//                        }
//                    },
//                    {
//                        selector: 'edge',
//                        style: {
//                            'width': 2,
//                            'line-color': '#666',
//                            'target-arrow-color': '#666',
//                            'target-arrow-shape': 'triangle',
//                            'curve-style': 'bezier'
//                        }
//                    }
//                ],
//                layout: {
//                    name: 'breadthfirst',
//                    directed: true,
//                    // 💡 Key Change: Increase layout padding to make the graph larger initially.
//                    padding: 100,
//                    spacingFactor: 1.5
//                }
//                // 💡 Removed the cy.ready() and cy.fit() block.
//                // The layout's padding will handle the initial size.
//            });
//        });
//});

//document.addEventListener("DOMContentLoaded", function () {
//    const mainContainer = document.getElementById('flowchart');
//    mainContainer.innerHTML = ""; // Clear existing content

//    fetch('/Employee/GetAllUIlist')
//        .then(res => res.json())
//        .then(data => {
//            // Step 1: Get all root nodes where uI_Part_linked_to = 0
//            const rootNodes = data.filter(item => item.uI_Part_linked_to === 0);

//            // Step 2: Create a separate flowchart container for each root
//            rootNodes.forEach(root => {
//                const wrapper = document.createElement('div');
//                wrapper.className = 'flowchart-wrapper';
//                wrapper.style.border = "1px solid #ccc";
//                wrapper.style.margin = "20px 0";
//                wrapper.style.padding = "10px";
//                wrapper.style.background = "#f9f9f9";

//                const title = document.createElement('h4');
//                title.textContent = root.uI_Name_Label;
//                title.style.marginBottom = "10px";
//                wrapper.appendChild(title);

//                const cyContainer = document.createElement('div');
//                cyContainer.className = 'cy-container';
//                cyContainer.style.width = "100%";
//                // Dynamically adjust height for longer flowcharts
//                if (root.uI_Name_Label === 'Masters' || root.uI_Name_Label === 'Company Settings') {
//                    cyContainer.style.height = "700px"; // Increased height
//                } else {
//                    cyContainer.style.height = "500px"; // Default height
//                }
//                wrapper.appendChild(cyContainer);
//                mainContainer.appendChild(wrapper);

//                const elements = [];
//                elements.push({ data: { id: String(root.uiListId), label: root.uI_Name_Label } });

//                function addChildren(parentId) {
//                    const children = data.filter(item => item.uI_Part_linked_to === parentId);
//                    children.forEach(child => {
//                        elements.push({ data: { id: String(child.uiListId), label: child.uI_Name_Label } });
//                        elements.push({ data: { source: String(parentId), target: String(child.uiListId) } });
//                        addChildren(child.uiListId);
//                    });
//                }
//                addChildren(root.uiListId);

//                const cy = cytoscape({
//                    container: cyContainer,
//                    elements: elements,
//                    style: [
//                        {
//                            selector: 'node',
//                            style: {
//                                'background-color': '#e3f2fd',
//                                'label': 'data(label)',
//                                'color': '#111',
//                                'text-valign': 'center',
//                                'text-halign': 'center',
//                                'font-size': 12, // Reduced font size to fit more text
//                                'border-width': 2,
//                                'border-color': '#1565c0',
//                                'shape': 'round-rectangle',
//                                'padding': '5px', // Reduced padding
//                                'text-wrap': 'wrap',
//                                'text-max-width': '80px', // Reduced width for each node
//                                'width': 'label',
//                                'height': 'label',
//                            }
//                        },
//                        {
//                            selector: 'edge',
//                            style: {
//                                'width': 2,
//                                'line-color': '#666',
//                                'target-arrow-color': '#666',
//                                'target-arrow-shape': 'triangle',
//                                'curve-style': 'bezier'
//                            }
//                        }
//                    ],
//                    layout: {
//                        name: 'dagre',
//                        rankDir: 'LR',
//                        rankSep: 500, // Restored to a more balanced value
//                        nodeSep: 20, // Increased vertical space to fix overcrowding
//                        padding: 30
//                    }
//                });

//                cy.ready(() => {
//                    cy.fit(cy.nodes(), 50);
//                    cy.center();
//                });
//            });
//        });
//});
// 2nd level chunks 
//document.addEventListener("DOMContentLoaded", function () {
//    const mainContainer = document.getElementById('flowchart');
//    mainContainer.innerHTML = ""; // Clear existing content

//    fetch('/Employee/GetAllUIlist')
//        .then(res => res.json())
//        .then(data => {
//            // Find all root nodes (uI_Part_linked_to = 0)
//            const rootNodes = data.filter(item => item.uI_Part_linked_to === 0);

//            rootNodes.forEach(root => {
//                if (root.uI_Name_Label === 'Masters') {
//                    // If the root is 'Masters', iterate through its children and create separate flowcharts
//                    const mastersChildren = data.filter(item => item.uI_Part_linked_to === root.uiListId);
//                    mastersChildren.forEach(childNode => {
//                        createFlowchartDiv(childNode, data, mainContainer, childNode.uI_Name_Label);
//                    });
//                } else {
//                    // For all other root nodes (like 'Company Settings'), create one full flowchart
//                    createFlowchartDiv(root, data, mainContainer, root.uI_Name_Label);
//                }
//            });
//        });

//    // Helper function to create and render a single flowchart
//    function createFlowchartDiv(rootNode, allData, containerElement, titleText) {
//        const wrapper = document.createElement('div');
//        wrapper.className = 'flowchart-wrapper';
//        wrapper.style.border = "1px solid #ccc";
//        wrapper.style.margin = "20px 0";
//        wrapper.style.padding = "10px";
//        wrapper.style.background = "#f9f9f9";

//        const title = document.createElement('h4');
//        title.textContent = titleText;
//        title.style.marginBottom = "10px";
//        wrapper.appendChild(title);

//        const cyContainer = document.createElement('div');
//        cyContainer.className = 'cy-container';
//        cyContainer.style.width = "100%";
//        cyContainer.style.height = "500px";
//        wrapper.appendChild(cyContainer);
//        containerElement.appendChild(wrapper);

//        const elements = [];
//        elements.push({ data: { id: String(rootNode.uiListId), label: rootNode.uI_Name_Label } });

//        // Recursively add all children and their sub-children for this rootNode
//        function addChildren(parentId) {
//            const children = allData.filter(item => item.uI_Part_linked_to === parentId);
//            children.forEach(child => {
//                elements.push({ data: { id: String(child.uiListId), label: child.uI_Name_Label } });
//                elements.push({ data: { source: String(parentId), target: String(child.uiListId) } });
//                addChildren(child.uiListId);
//            });
//        }
//        addChildren(rootNode.uiListId);

//        const cy = cytoscape({
//            container: cyContainer,
//            elements: elements,
//            style: [
//                {
//                    selector: 'node',
//                    style: {
//                        'background-color': '#e3f2fd',
//                        'label': 'data(label)',
//                        'color': '#111',
//                        'text-valign': 'center',
//                        'text-halign': 'center',
//                        'font-size': 12,
//                        'border-width': 2,
//                        'border-color': '#1565c0',
//                        'shape': 'round-rectangle',
//                        'padding': '5px',
//                        'text-wrap': 'wrap',
//                        'text-max-width': '80px',
//                        'width': 'label',
//                        'height': 'label',
//                    }
//                },
//                {
//                    selector: 'edge',
//                    style: {
//                        'width': 2,
//                        'line-color': '#666',
//                        'target-arrow-color': '#666',
//                        'target-arrow-shape': 'triangle',
//                        'curve-style': 'bezier'
//                    }
//                }
//            ],
//            layout: {
//                name: 'dagre',
//                rankDir: 'LR',
//                rankSep: 80,
//                nodeSep: 20,
//                padding: 30
//            }
//        });

//        cy.ready(() => {
//            cy.fit(cy.nodes(), 50);
//            cy.center();
//        });
//    }
//});
/*
document.addEventListener("DOMContentLoaded", function () {
    const mainContainer = document.getElementById('flowchart');
    mainContainer.innerHTML = ""; // Clear existing content

    fetch('/Employee/GetAllUIlist')
        .then(res => res.json())
        .then(data => {
            const rootNodes = data.filter(item => item.uI_Part_linked_to === 0);

            rootNodes.forEach(root => {
                if (root.uI_Name_Label === 'Masters' || root.uI_Name_Label === 'Business Process') {
                    const children = data.filter(item => item.uI_Part_linked_to === root.uiListId);
                    children.forEach(childNode => {
                        // Pass the parent's name to construct the heading
                        createFlowchartDiv(childNode, data, mainContainer, root.uI_Name_Label, childNode.uI_Name_Label, root);
                    });
                } else {
                    // For other root nodes, there is no parent in the heading
                    createFlowchartDiv(root, data, mainContainer, null, root.uI_Name_Label, null);
                }
            });
        });

    // Helper function to create and render a single flowchart
    function createFlowchartDiv(rootNode, allData, containerElement, parentTitle, childTitle, parentNode) {
        const wrapper = document.createElement('div');
        wrapper.className = 'flowchart-wrapper';
        wrapper.style.border = "1px solid #ccc";
        wrapper.style.margin = "20px 0";
        wrapper.style.padding = "10px";
        wrapper.style.background = "#f9f9f9";

        const title = document.createElement('h4');
        if (parentTitle) {
            title.textContent = `${parentTitle} -> ${childTitle}`;
        } else {
            title.textContent = childTitle;
        }
        title.style.marginBottom = "10px";
        wrapper.appendChild(title);

        const cyContainer = document.createElement('div');
        cyContainer.className = 'cy-container';
        cyContainer.style.width = "100%";
        cyContainer.style.height = "500px";
        wrapper.appendChild(cyContainer);
        containerElement.appendChild(wrapper);

        const elements = [];

        if (parentNode) {
            elements.push({ data: { id: String(parentNode.uiListId), label: parentNode.uI_Name_Label } });
            elements.push({ data: { id: String(rootNode.uiListId), label: rootNode.uI_Name_Label } });
            elements.push({ data: { source: String(parentNode.uiListId), target: String(rootNode.uiListId) } });
        } else {
            elements.push({ data: { id: String(rootNode.uiListId), label: rootNode.uI_Name_Label } });
        }

        function addChildren(parentId) {
            const children = allData.filter(item => item.uI_Part_linked_to === parentId);
            children.forEach(child => {
                elements.push({ data: { id: String(child.uiListId), label: child.uI_Name_Label } });
                elements.push({ data: { source: String(parentId), target: String(child.uiListId) } });
                addChildren(child.uiListId);
            });
        }

        addChildren(rootNode.uiListId);

        const cy = cytoscape({
            container: cyContainer,
            elements: elements,
            style: [
                {
                    selector: 'node',
                    style: {
                        'background-color': '#e3f2fd',
                        'label': 'data(label)',
                        'color': '#111',
                        'text-valign': 'center',
                        'text-halign': 'center',
                        'font-size': 12,
                        'border-width': 2,
                        'border-color': '#1565c0',
                        'shape': 'round-rectangle',
                        'padding': '5px',
                        'text-wrap': 'wrap',
                        'text-max-width': '80px',
                        'width': 'label',
                        'height': 'label',
                    }
                },
                {
                    selector: 'edge',
                    style: {
                        'width': 2,
                        'line-color': '#666',
                        'target-arrow-color': '#666',
                        'target-arrow-shape': 'triangle',
                        'curve-style': 'bezier'
                    }
                }
            ],
            layout: {
                name: 'dagre',
                rankDir: 'LR',
                rankSep: 80,
                nodeSep: 20,
                padding: 30
            }
        });

        cy.ready(() => {
            cy.fit(cy.nodes(), 50);
            cy.center();
        });
    }
});
*/
//document.addEventListener("DOMContentLoaded", function () {
//    const mainContainer = document.getElementById('flowchart');
//    mainContainer.innerHTML = ""; // Clear existing content

//    fetch('/Employee/GetAllUIlist')
//        .then(res => res.json())
//        .then(data => {
//            const rootNodes = data.filter(item => item.uI_Part_linked_to === 0);

//            rootNodes.forEach(root => {
//                const isSpecialRoot = (root.uI_Name_Label === 'Masters' || root.uI_Name_Label === 'Business Process');

//                if (isSpecialRoot) {
//                    const children = data.filter(item => item.uI_Part_linked_to === root.uiListId);

//                    children.forEach(childNode => {
//                        createFlowchartDiv(childNode, data, mainContainer, root, childNode);
//                    });
//                } else {
//                    // For all other root nodes, create a single full flowchart
//                    createFlowchartDiv(root, data, mainContainer, null, root);
//                }
//            });
//        });

//    // Helper function to create and render a single flowchart
//    function createFlowchartDiv(startNode, allData, containerElement, parentNode, titleNode) {
//        const wrapper = document.createElement('div');
//        wrapper.className = 'flowchart-wrapper';
//        wrapper.style.border = "1px solid #ccc";
//        wrapper.style.margin = "20px 0";
//        wrapper.style.padding = "10px";
//        wrapper.style.background = "#f9f9f9";

//        const title = document.createElement('h4');
//        if (parentNode) {
//            title.textContent = `${parentNode.uI_Name_Label} -> ${titleNode.uI_Name_Label}`;
//        } else {
//            title.textContent = titleNode.uI_Name_Label;
//        }
//        title.style.marginBottom = "10px";
//        wrapper.appendChild(title);

//        const cyContainer = document.createElement('div');
//        cyContainer.className = 'cy-container';
//        cyContainer.style.width = "100%";
//        cyContainer.style.height = "500px";
//        wrapper.appendChild(cyContainer);
//        containerElement.appendChild(wrapper);

//        const elements = [];

//        if (parentNode) {
//            // Add and connect the parent node (e.g., "Masters" or "Business Process")
//            elements.push({ data: { id: String(parentNode.uiListId), label: parentNode.uI_Name_Label } });
//            elements.push({ data: { id: String(startNode.uiListId), label: startNode.uI_Name_Label } });
//            elements.push({ data: { source: String(parentNode.uiListId), target: String(startNode.uiListId) } });
//        } else {
//            // If no parent node, just add the start node
//            elements.push({ data: { id: String(startNode.uiListId), label: startNode.uI_Name_Label } });
//        }

//        function addChildren(parentId) {
//            const children = allData.filter(item => item.uI_Part_linked_to === parentId);
//            children.forEach(child => {
//                elements.push({ data: { id: String(child.uiListId), label: child.uI_Name_Label } });
//                elements.push({ data: { source: String(parentId), target: String(child.uiListId) } });
//                addChildren(child.uiListId);
//            });
//        }

//        addChildren(startNode.uiListId);

//        const cy = cytoscape({
//            container: cyContainer,
//            elements: elements,
//            style: [
//                {
//                    selector: 'node',
//                    style: {
//                        'background-color': '#e3f2fd',
//                        'label': 'data(label)',
//                        'color': '#111',
//                        'text-valign': 'center',
//                        'text-halign': 'center',
//                        'font-size': 12,
//                        'border-width': 2,
//                        'border-color': '#1565c0',
//                        'shape': 'round-rectangle',
//                        'padding': '5px',
//                        'text-wrap': 'wrap',
//                        'text-max-width': '80px',
//                        'width': 'label',
//                        'height': 'label',
//                    }
//                },
//                {
//                    selector: 'edge',
//                    style: {
//                        'width': 2,
//                        'line-color': '#666',
//                        'target-arrow-color': '#666',
//                        'target-arrow-shape': 'triangle',
//                        'curve-style': 'bezier'
//                    }
//                }
//            ],
//            layout: {
//                name: 'dagre',
//                rankDir: 'LR',
//                rankSep: 80,
//                nodeSep: 20,
//                padding: 30
//            }
//        });

//        cy.ready(() => {
//            cy.fit(cy.nodes(), 50);
//            cy.center();
//        });
//    }
//});

document.addEventListener("DOMContentLoaded", function () {
    const mainContainer = document.getElementById('flowchart');
    mainContainer.innerHTML = ""; // Clear existing content

    fetch('/Employee/GetAllUIlist')
        .then(res => res.json())
        .then(data => {
            const rootNodes = data.filter(item => item.uI_Part_linked_to === 0);

            rootNodes.forEach(root => {
                const isSpecialRoot = (root.uI_Name_Label === 'Masters' || root.uI_Name_Label === 'Business Process');

                if (isSpecialRoot) {
                    const children = data.filter(item => item.uI_Part_linked_to === root.uiListId);

                    children.forEach(childNode => {
                        // Check if the child is 'Process Planning'
                        if (childNode.uI_Name_Label === 'Process Planning') {
                            const processPlanningChildren = data.filter(item => item.uI_Part_linked_to === childNode.uiListId);

                            // Get the specific children that need separate chunks
                            const specialChildren = processPlanningChildren.filter(ppChild => ppChild.uI_Name_Label === 'WIP Control' || ppChild.uI_Name_Label === 'Simulate');

                            // Create a single flowchart for the main 'Process Planning' section, excluding the special children
                            createFlowchartDiv(childNode, data, mainContainer, root, childNode, specialChildren.map(c => c.uI_Name_Label));

                            // Create a separate div chunk for 'WIP Control' and 'Simulate'
                            specialChildren.forEach(ppChild => {
                                createFlowchartDiv(ppChild, data, mainContainer, childNode, ppChild);
                            });
                        } else {
                            // Default case for other children of Masters and Business Process
                            createFlowchartDiv(childNode, data, mainContainer, root, childNode);
                        }
                    });
                } else {
                    // For all other root nodes, create a single full flowchart
                    createFlowchartDiv(root, data, mainContainer, null, root);
                }
            });
        });

    // Helper function to create and render a single flowchart
    function createFlowchartDiv(startNode, allData, containerElement, parentNode, titleNode, nodesToExclude = []) {
        const wrapper = document.createElement('div');
        wrapper.className = 'flowchart-wrapper';
        wrapper.style.border = "1px solid #ccc";
        wrapper.style.margin = "20px 0";
        wrapper.style.padding = "10px";
        wrapper.style.background = "#f9f9f9";

        const title = document.createElement('h4');
        if (parentNode && parentNode.uI_Name_Label !== 'Business Process' && parentNode.uI_Name_Label !== 'Masters') {
            // For grandchildren, include the full path
            const grandparentNode = allData.find(item => item.uiListId === parentNode.uI_Part_linked_to);
            if (grandparentNode) {
                title.textContent = `${grandparentNode.uI_Name_Label} -> ${parentNode.uI_Name_Label} -> ${titleNode.uI_Name_Label}`;
            } else {
                title.textContent = `${parentNode.uI_Name_Label} -> ${titleNode.uI_Name_Label}`;
            }
        } else if (parentNode) {
            title.textContent = `${parentNode.uI_Name_Label} -> ${titleNode.uI_Name_Label}`;
        } else {
            title.textContent = titleNode.uI_Name_Label;
        }
        title.style.marginBottom = "10px";
        wrapper.appendChild(title);

        const cyContainer = document.createElement('div');
        cyContainer.className = 'cy-container';
        cyContainer.style.width = "100%";
        cyContainer.style.height = "500px";
        wrapper.appendChild(cyContainer);
        containerElement.appendChild(wrapper);

        const elements = [];

        if (parentNode) {
            elements.push({ data: { id: String(parentNode.uiListId), label: parentNode.uI_Name_Label } });
            elements.push({ data: { id: String(startNode.uiListId), label: startNode.uI_Name_Label } });
            elements.push({ data: { source: String(parentNode.uiListId), target: String(startNode.uiListId) } });
        } else {
            elements.push({ data: { id: String(startNode.uiListId), label: startNode.uI_Name_Label } });
        }

        function addChildren(parentId) {
            let children = allData.filter(item => item.uI_Part_linked_to === parentId);

            // Filter out the nodes that should be excluded from this specific chart
            if (nodesToExclude.length > 0) {
                children = children.filter(c => !nodesToExclude.includes(c.uI_Name_Label));
            }

            children.forEach(child => {
                elements.push({ data: { id: String(child.uiListId), label: child.uI_Name_Label } });
                elements.push({ data: { source: String(parentId), target: String(child.uiListId) } });
                addChildren(child.uiListId);
            });
        }

        addChildren(startNode.uiListId);

        const cy = cytoscape({
            container: cyContainer,
            elements: elements,
            style: [
                {
                    selector: 'node',
                    style: {
                        'background-color': '#e3f2fd',
                        'label': 'data(label)',
                        'color': '#111',
                        'text-valign': 'center',
                        'text-halign': 'center',
                        'font-size': 12,
                        'border-width': 2,
                        'border-color': '#1565c0',
                        'shape': 'round-rectangle',
                        'padding': '5px',
                        'text-wrap': 'wrap',
                        'text-max-width': '80px',
                        'width': 'label',
                        'height': 'label',
                    }
                },
                {
                    selector: 'edge',
                    style: {
                        'width': 2,
                        'line-color': '#666',
                        'target-arrow-color': '#666',
                        'target-arrow-shape': 'triangle',
                        'curve-style': 'bezier'
                    }
                }
            ],
            layout: {
                name: 'dagre',
                rankDir: 'LR',
                rankSep: 80,
                nodeSep: 20,
                padding: 30
            },
            panningEnabled: true, // You may want to keep panning enabled
            userPanningEnabled: true
        });

        cy.ready(() => {
            cy.fit(cy.nodes(), 50);
            cy.center();
        });
    }
});

function saveEmployee(rowData) {
    api.post("/Employee/PostEmployee", rowData)
        .then((data) => {
            $("#EPEmpId").val(data.employee_ID);
            $("#UiAccessEEmplid").val(data.employee_ID);

            LoadEmployee();
            LoadEmplUiById();

            // Prepare data for account registration
            var userrowData = {
                username: data.email,
                email: data.email,
                firstName: data.employee_name,
                lastName: data.employee_name,
                password: data.password,
                phoneNumber: data.phone,
                tenantId: data.tenantId
            };

            const ipAddress = window.location.hostname;
            alert("Employee Saved Successfully!");
            isFormDirty = false;

            // Register employee
            api.post(`http://${ipAddress}:9003/account/Register`, userrowData, {
                headers: {
                    "Content-Type": "application/json",
                    "Accept": "application/json",
                },
            })
                .then((response) => {
                    console.log("Registration Success:", response);
                })
                .catch((error) => {
                    console.error("Error during account registration:", error);
                });
        })
        .catch((error) => {
            console.error("Error saving employee:", error);
        });
}


$(function () {
    //Search Designation --
    LoadEmployee();
    LoadRoleUiAll();
    $("#SearchEmplno").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#EmployeeGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#EmployeeGrid tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $tableBody.append(noRecordsRow);
        } else {
            $tableBody.find(".norecordsfound").remove();
        }
    });
    $("#SearchEmplName").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#EmployeeGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#EmployeeGrid tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $tableBody.append(noRecordsRow);
        } else {
            $tableBody.find(".norecordsfound").remove();
        }
    });
    $("#SearchLoc").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#EmployeeGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#EmployeeGrid tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $tableBody.append(noRecordsRow);
        } else {
            $tableBody.find(".norecordsfound").remove();
        }
    });
    $("#SearchEmailId").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#EmployeeGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#EmployeeGrid tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $tableBody.append(noRecordsRow);
        } else {
            $tableBody.find(".norecordsfound").remove();
        }
    });
    $("#SearchDateOfJoin").on("change", function () {
        var selectedDate = $(this).val(); 
        var formattedSelectedDate = new Date(selectedDate);
        var month = (formattedSelectedDate.getMonth() + 1).toString().padStart(2, '0'); // Months are 0-based
        var day = formattedSelectedDate.getDate().toString().padStart(2, '0');
        var year = formattedSelectedDate.getFullYear();
        var finalSelectedDate = month + '/' + day + '/' + year; 
        $("#EmployeeGrid tbody tr").filter(function () {
            var rowDate = $(this).children("td").eq(2).text();
            if (rowDate === finalSelectedDate || selectedDate === "") {
                $(this).show(); 
            } else {
                $(this).hide(); 
            }
        });
    });
    $("#UPPartOf").select2({
        dropdownParent: $("#addUi")
    });

   
    loadDesignation();
    loadDepartment();
    loadLoaction();


    $('#addEmployee input, #addEmployee select, #addEmployee textarea').on('change keyup', function () {
        if (!isInitializing) {
            isFormDirty = true;
        }
    });

    $('#addEmployee').on('hide.bs.modal', function (event) {

        if (isFormDirty) {
            const confirmClose = confirm("Data not saved… Exit?");
            if (!confirmClose) {
                event.preventDefault(); // Stop closing
                return false;
            } else {
                isFormDirty = false; // Reset if confirmed
            }
        }
        $("#error-password").text(" ").css("color", "red");
        $("#error-username").text(" ").css("color", "red");
        $("#EPEmpId").val(''); $("#UiAccessEEmplid").val('');
        $("#EPEmpNo").val('');
        $("#EPEmpName").val('');
        $("#EPDateOfJoin").val('');
        $("#EPDateOfLeave").val('');
        $("#EPCell").val('');
        $("#EPEmail").val('');
        $("#EPAddress").val('');
        $("#EPContPer").val('');
        $("#EPContNo").val('');
        $("#EPEmpId").val('');
        $("#EPEmpRoles").val('');
        $("#EPEmpRoleid").val('');
       // $("#EPUserName").val('');
        $("#EPCfmPassword").val('');
        $("#EPPassword").val('');
        $("#EPDes").val(0).change(); // Set designation and trigger change
        $("#EPDept").val(0).change(); // Set department and trigger change
        $("#EPloc").val(0).change();
        $("#EPResignChk").prop("checked", false);
        $("#EPHeadOrg").prop("checked", false);
        var EPEmpNo = document.getElementById('EPEmpNo');
        EPEmpNo.style.border = '';
        var EPDateOfJoin = document.getElementById('EPDateOfJoin');
        EPDateOfJoin.style.border = '';
        var EPEmpName = document.getElementById('EPEmpName');
        EPEmpName.style.border = '';
        var EPCell = document.getElementById('EPCell');
        EPCell.style.border = '';
        var EPEmail = document.getElementById('EPEmail');
        EPEmail.style.border = '';
        var EPAddress = document.getElementById('EPAddress');
        EPAddress.style.border = '';
        var EPDes = document.getElementById('EPDes');
        EPDes.style.border = '';
        var EPDept = document.getElementById('EPDept');
        EPDept.style.border = '';
        var EPloc = document.getElementById('EPloc');
        EPloc.style.border = '';
        var EPDateOfLeave = document.getElementById('EPDateOfLeave');
        EPDateOfLeave.style.border = '';
        var EPEmpRoleid = document.getElementById('EPEmpRoles');
        EPEmpRoleid.style.border = '';
        //var EPUserName = document.getElementById('EPUserName');
        //EPUserName.style.border = '';
        var EPPassword = document.getElementById('EPPassword');
        EPPassword.style.border = '';
        var EPCfmPassword = document.getElementById('EPCfmPassword');
        EPCfmPassword.style.border = '';
        var EPContPer = document.getElementById('EPContPer');
        EPContPer.style.border = '';
        var EPContNo = document.getElementById('EPContNo');
        EPContNo.style.border = '';
        var UiAccessRMenu1 = $('#EPDept');
        UiAccessRMenu1.html('');
        var EPRoleReportTo = document.getElementById('EPRoleReportTo');
        EPRoleReportTo.style.border = '';
    });
    $('#addEmployee').on('show.bs.modal', function (event) {
        isInitializing = true;  // prevent dirty flag during prepopulation
        isFormDirty = false;
        LoadDepartments();
        loadLevels();
        var tablebody = $("#EmpDeptLinkGrid tbody");
        $(tablebody).html("");
        $("#ReportToDept").val('');
        //$("#DateOfLeaveDIv").hide();
        $("#RoleReportDIv").show();
        $("#EPDept").show();
        $("#lblDept").show();
        $("#lblRole").show();
        $("#EPEmpRoles").show();
        $("#empno-error").text("").css("color", "red");
        $("#email-error").text("").css("color", "red");
        var relatedTarget = $(event.relatedTarget);
        var employee_ID = relatedTarget.data("employeeid");
        var employeeno = relatedTarget.data("employeeno");
        var emplname = relatedTarget.data("emplname");
        var dept = relatedTarget.data("dept");
        var desg = relatedTarget.data("desg");
        var cell = relatedTarget.data("cell");
        var email = relatedTarget.data("email");
        var uname = relatedTarget.data("uname");
        var rolereport = relatedTarget.data("rolereport");
        var headoforg = relatedTarget.data("headoforg");
        var eroleids = relatedTarget.data("eroleids");
        var passwd = relatedTarget.data("passwd");
        var dateofjoin = relatedTarget.data("dateofjoin");
        var chkresign = relatedTarget.data("chkresign");
        var date_Of_Resigning = relatedTarget.data("dateofresigning");
        var plant_Id = relatedTarget.data("plantid");
        var emerg_Contact_Name = relatedTarget.data("emergcontactname");
        var emerg_Contact_No = relatedTarget.data("emergcontactno");
        var address = relatedTarget.data("address");
        if (employee_ID > 0) {
            LoadDeptEmp(employee_ID);
            $("#EPEmpId").val(employee_ID);
            $("#EPEmpNo").val(employeeno);
            $("#EPEmpName").val(emplname);
            var formattedDate = dateofjoin.split("T")[0]; 
            $("#EPDateOfJoin").val(formattedDate);
            $("#EPCell").val(cell);
            $("#EPPassword").val(passwd);
           // $("#EPUserName").val(uname);
            $("#EPCfmPassword").val(passwd);
            $("#EPEmail").val(email);
            $("#EPAddress").val(address);
            $("#EPContPer").val(emerg_Contact_Name);
            $("#EPContNo").val(emerg_Contact_No);
            $("#EPDes").val(desg).change(); // Set designation and trigger change // Set department and trigger change
            $("#EPloc").val(plant_Id).change();
            if (date_Of_Resigning && typeof date_Of_Resigning === "string") {
                var formattedDateleave = date_Of_Resigning.split("T")[0];
                $("#EPDateOfLeave").val(formattedDateleave);
            }
            if (chkresign === "Y") {
                $("#EPResignChk").prop("checked", true);
                //$("#DateOfLeaveDIv").show();
            } else {
                $("#EPResignChk").prop("checked", false);
            }
            if (headoforg === "Y") {
                $("#EPHeadOrg").prop("checked", true);
                $("#RoleReportDIv").hide();
                $("#lblDept").hide();
                $("#EPDept").hide();
                $("#lblRole").hide();
                $("#EPEmpRoles").hide();
            } else {
                $("#EPHeadOrg").prop("checked", false);
                $("#RoleReportDIv").show();
                $("#lblDept").show();
                $("#EPDept").show();
                $("#lblRole").show();
                $("#EPEmpRoles").show();
            }
            $("#UiAccessEEmplid").val(employee_ID);
            var UiAccessRMenu1 = $('#EPDept');
            UiAccessRMenu1.html('');
            const filteredData = Departments.filter(item => item.plantId === plant_Id);
            div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
            UiAccessRMenu1.append(div_data);
            for (var i = 0; i < filteredData.length; i++) {
                div_data = "<option value='" + filteredData[i].departmentId + "'>" + filteredData[i].name + "</option>";
                UiAccessRMenu1.append(div_data);
            }
            $("#EPDept").val(dept).change();
            var OrgEmpl = $('#EPRoleReportTo');
            OrgEmpl.html('');
            api.get("/Employee/GetAllEmployee").then((data) => {
                const filteredData = data.filter(item => item.headOfDepartment === "Y");
                div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
                OrgEmpl.append(div_data);
                OrgRoleReport.append(div_data);
                for (i = 0; i < filteredData.length; i++) {
                    div_data = "<option value='" + filteredData[i].employee_ID + "'>" + filteredData[i].employee_name + "</option>";
                    OrgEmpl.append(div_data);
                    OrgRoleReport.append(div_data);
                }
                $("#EPRoleReportTo").val(rolereport).change();
                setTimeout(function () {
                    isInitializing = false;  // re-enable dirty tracking
                }, 600);
            }).catch((error) => {
            });
        } else {
            var OrgEmpl = $('#EPRoleReportTo');
            OrgEmpl.html('');
            api.get("/Employee/GetAllEmployee").then((data) => {
                const filteredData = data.filter(item => item.headOfDepartment === "Y");
                div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
                OrgEmpl.append(div_data);
                OrgRoleReport.append(div_data);
                setTimeout(function () {
                    isInitializing = false;  // re-enable dirty tracking
                }, 200);
                for (i = 0; i < data.length; i++) {
                    div_data = "<option value='" + filteredData[i].employee_ID + "'>" + filteredData[i].employee_name + "</option>";
                    OrgEmpl.append(div_data);
                    OrgRoleReport.append(div_data);
                }
            }).catch((error) => {
            });
        }
        LoadEmplUiById();
        loadSelectMenusForEmpl();
        loadSelectEmplPermission();
    });

    $('#EPResignChk').on('click', function () {
        if ($(this).is(':checked')) {
            //$("#DateOfLeaveDIv").show();
        } else {
            //$("#DateOfLeaveDIv").hide();
        }
    });
    $('#EPDept').on('change', function () {
        var deptid = $("#EPDept").val();
        //api.get("/Employee/GetAllOrgChart").then((data) => {
        //    const filteredData = data.filter(item => item.dept_ID === parseInt(deptid));
        //    $("#EPEmpRoleid").val(filteredData[0].roleIds);
        //    $("#EPEmpRoles").val(filteredData[0].roleName);
        //});
    });
    $('#EPHeadOrg').on('click', function () {
        if ($(this).is(':checked')) {
            $("#RoleReportDIv").hide();
            $("#lblDept").hide();
            $("#EPDept").hide();
            $("#lblRole").hide();
            $("#EPEmpRoles").hide();
        } else {
            $("#RoleReportDIv").show();
            $("#lblDept").show();
            $("#EPDept").show();
            $("#lblRole").show();
            $("#EPEmpRoles").show();
        }
    });
    $('#EPEmail').on('keyup', function () {
        var email = $(this).val();
        var emailRegex = /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
        if (emailRegex.test(email)) {
            $("#email-error").text("").css("color", "red");
        } else {
            $("#email-error").text("Please enter a valid email address.").css("color", "red");
        }
    });
    $("#EmployeeSave").secureClick(function () {
        //   //debugger;
        var EPEmpId = parseInt($("#EPEmpId").val());
        var EPEmpNo = $("#EPEmpNo").val();
        var EPEmpName = $("#EPEmpName").val();
        var EPPassword = $("#EPPassword").val();
       // var EPUserName = $("#EPUserName").val();
        var EPCfmPassword = $("#EPCfmPassword").val();
        var EPDateOfJoin = $("#EPDateOfJoin").val();
        var EPCell = $("#EPCell").val();
        var EPEmail = $("#EPEmail").val();
        var EPEmpRoleid = $("#EPEmpRoleid").val();
        var EPAddress = $("#EPAddress").val();
        var EPContPer = $("#EPContPer").val();
        var EPContNo = $("#EPContNo").val();
        var EPDateOfLeave = $("#EPDateOfLeave").val();
        var EPDes = parseInt($("#EPDes").val());
        var EPDept = parseInt($("#EPDept").val());
        var EPloc = parseInt($("#EPloc").val());
        var EPRoleReportTo = parseInt($("#EPRoleReportTo").val());
        var checkbox = document.getElementById("EPResignChk");
        var EPHeadOrg = document.getElementById("EPHeadOrg");
        var EPResignChk = 'N';
        var EPHeadOrgChk = 'N';
        if (EPEmpNo.length <= 0) {
            var newNamevalidate = document.getElementById('EPEmpNo');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('EPEmpNo');
            newNamevalidate.style.border = '';
        }
        if (EPDateOfJoin.length <= 0) {
            var newNamevalidate = document.getElementById('EPDateOfJoin');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('EPDateOfJoin');
            newNamevalidate.style.border = '';
        }
        if (EPEmpName.length <= 0) {
            var newNamevalidate = document.getElementById('EPEmpName');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('EPEmpName');
            newNamevalidate.style.border = '';
        }
        if (EPDes === 0) {
            var newNamevalidate = document.getElementById('EPDes');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('EPDes');
            newNamevalidate.style.border = '';
        }
        //if (EPloc === 0) {
        //    var newNamevalidate = document.getElementById('EPloc');
        //    newNamevalidate.style.border = '2px solid red';
        //    return false;
        //} else {
        //    var newNamevalidate = document.getElementById('EPloc');
        //    newNamevalidate.style.border = '';
        //}
        if (EPCell.length <= 0) {
            var newNamevalidate = document.getElementById('EPCell');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('EPCell');
            newNamevalidate.style.border = '';
        }
        if (EPEmail.length <= 0) {
            var newNamevalidate = document.getElementById('EPEmail');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('EPEmail');
            newNamevalidate.style.border = '';
        }
        //var regex = /^[a-zA-Z0-9]+$/;

        //if (!regex.test(EPUserName)) {
        //    $("#error-username").text("Username must contain only letters, digits and cannot contain spaces.").css("color", "red");
        //    return false;
        //} else {
        //    $("#error-username").text(" ").css("color", "red");
        //}
        //var regexpassUpper = /[A-Z]/;
        //var regexpassLower = /[a-z]/;
        //if (!regexpassUpper.test(EPCfmPassword)) {
        //    $("#error-password").text("Password must contain at least one capital letter.").css("color", "red");
        //    return false;
        //} else if (!regexpassLower.test(EPCfmPassword)) {
        //    $("#error-password").text("Password must contain at least one lowercase letter.").css("color", "red");
        //    return false;
        //} else {
        //    $("#error-password").text(" ").css("color", "red");
        //}
        //if (EPCfmPassword != EPPassword) {
        //    var newNamevalidate = document.getElementById('EPCfmPassword');
        //    newNamevalidate.style.border = '2px solid red';
        //    return false;
        //} else {
        //    var newNamevalidate = document.getElementById('EPCfmPassword');
        //    newNamevalidate.style.border = '';
        //}
        //if (EPCfmPassword.length <= 6) {
        //    $("#error-password").text("Password must contain at least one capital letter.").css("color", "red");
        //    var newNamevalidate = document.getElementById('EPCfmPassword');
        //    newNamevalidate.style.border = '2px solid red';
        //    return false;
        //} else {
        //    $("#error-password").text(" ").css("color", "red");
        //    var newNamevalidate = document.getElementById('EPCfmPassword');
        //    newNamevalidate.style.border = '';
        //}
        if (EPAddress.length <= 0) {
            //var newNamevalidate = document.getElementById('EPAddress');
            //newNamevalidate.style.border = '2px solid red';
            //return false;
            EPAddress = "-";
        } else {
            var newNamevalidate = document.getElementById('EPAddress');
            newNamevalidate.style.border = '';
        }
        //if (EPContPer.length <= 0) {
        //    var newNamevalidate = document.getElementById('EPContPer');
        //    newNamevalidate.style.border = '2px solid red';
        //    return false;
        //} else {
        //    var newNamevalidate = document.getElementById('EPContPer');
        //    newNamevalidate.style.border = '';
        //}
        //if (EPContNo.length <= 0) {
        //    var newNamevalidate = document.getElementById('EPContNo');
        //    newNamevalidate.style.border = '2px solid red';
        //    return false;
        //} else {
        //    var newNamevalidate = document.getElementById('EPContNo');
        //    newNamevalidate.style.border = '';
        //}
        if (EPHeadOrg.checked) {
            EPHeadOrgChk = 'Y';
        } else {
            //if (EPDept === 0) {
            //    var newNamevalidate = document.getElementById('EPDept');
            //    newNamevalidate.style.border = '2px solid red';
            //    return false;
            //} else {
            //    var newNamevalidate = document.getElementById('EPDept');
            //    newNamevalidate.style.border = '';
            //}
            //if (EPEmpRoleid.length <= 0) {
            //    var newNamevalidate = document.getElementById('EPEmpRoles');
            //    newNamevalidate.style.border = '2px solid red';
            //    return false;
            //} else {
            //    var newNamevalidate = document.getElementById('EPEmpRoles');
            //    newNamevalidate.style.border = '';
            //}
            //if (EPRoleReportTo == 0) {
            //    var newNamevalidate = document.getElementById('EPRoleReportTo');
            //    newNamevalidate.style.border = '2px solid red';
            //    return false;
            //} else {
            //    var newNamevalidate = document.getElementById('EPRoleReportTo');
            //    newNamevalidate.style.border = '';
            //}
        }
        if (checkbox.checked) {
            EPResignChk = 'Y';
        }
        if (EPDateOfLeave.length <= 0) {
            //var newNamevalidate = document.getElementById('EPDateOfLeave');
            //newNamevalidate.style.border = '2px solid red';
            //return false;
        } else {
            var inputDate = new Date(document.getElementById('EPDateOfLeave').value);
            var today = new Date();

            today.setHours(0, 0, 0, 0);

            if (inputDate > today) {
                var newNamevalidate = document.getElementById('EPDateOfLeave');
                newNamevalidate.style.border = '2px solid red';
                alert("Date of Leaving should be less than todays date");
                return false;
            } else {
                var newNamevalidate = document.getElementById('EPDateOfLeave');
                newNamevalidate.style.border = '';
            }
        }
        if (isNaN(EPEmpId)) {
            EPEmpId = 0;
        }
        var rowData = {
            employee_ID: EPEmpId,
            employee_name: EPEmpName,
            designation_Id: EPDes,
            employee_No: EPEmpNo,
            Date_Of_Joining: EPDateOfJoin,
            phone: EPCell,
            email: EPEmail,
            userName: EPEmail,
            roleIds: EPEmpRoleid,
            headOfDepartment: EPHeadOrgChk,
            roleReportTo: EPRoleReportTo,
            password: EPCfmPassword,
            residential_Address: EPAddress,
            emerg_Contact_Name: EPContPer,
            emerg_Contact_No: EPContNo,
            plant_Id: EPloc,
            home_Dept_Id: EPDept,
            employee_Resigned: EPResignChk,
            date_Of_Resigning: EPDateOfLeave
        };

      return api.getbulk("/Employee/GetUnique?empNo=" + EPEmpNo)
            .then((data) => {
                // If employee number already exists
                if (data !== true && EPEmpId === 0) {
                    $("#empno-error").text("This Employee No already exists.").css("color", "red");
                    return;
                } else {
                    $("#empno-error").text(""); // Clear previous error
                }

                // 🚀 Step 2: ONLY check email for NEW employee
                if (EPEmpId === 0) {
                    api.get("/Employee/GetAllEmployee")
                        .then((edata) => {
                            // Check if email already exists
                            const emailExists = edata.some(
                                (item) => item.email.toLowerCase() === EPEmail.toLowerCase()
                            );

                            if (emailExists) {
                                $("#email-error").text("This Email ID already exists.").css("color", "red");
                                return; // Stop execution if duplicate email found
                            } else {
                                $("#email-error").text(""); // Clear previous error
                                saveEmployee(rowData); // ✅ Proceed to save employee
                            }
                        })
                        .catch((error) => {
                            console.error("Error fetching employees:", error);
                        });
                } else {
                    // Editing an existing employee, skip email validation
                    saveEmployee(rowData);
                }
            })
            .catch((error) => {
                console.error("Error validating employee number:", error);
            });
    });

    $('#orgChart').on('show.bs.modal', function (event) {
        LoadOrgChart();
        loadSelectRole();
    });

    $("#OrgRole").select2({
        dropdownParent: $("#addOrgChart")
    });

    $("#SearchOrgloc").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#OrgChartGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#OrgChartGrid tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $tableBody.append(noRecordsRow);
        } else {
            $tableBody.find(".norecordsfound").remove();
        }
    });
    $("#SearchOrgDept").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#OrgChartGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#OrgChartGrid tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $tableBody.append(noRecordsRow);
        } else {
            $tableBody.find(".norecordsfound").remove();
        }
    });
    $("#SearchOrgRole").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#OrgChartGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#OrgChartGrid tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $tableBody.append(noRecordsRow);
        } else {
            $tableBody.find(".norecordsfound").remove();
        }
    });
    $("#SearchOrgEmp").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#OrgChartGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#OrgChartGrid tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $tableBody.append(noRecordsRow);
        } else {
            $tableBody.find(".norecordsfound").remove();
        }
    });
    $("#SearchOrgRRole").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#OrgChartGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#OrgChartGrid tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $tableBody.append(noRecordsRow);
        } else {
            $tableBody.find(".norecordsfound").remove();
        }
    });
    $("#SearchOrgLevel").on("change", function () {
        var value = $(this).val().toLowerCase();
        $("#OrgChartGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[5]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#OrgChartGrid tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $tableBody.append(noRecordsRow);
        } else {
            $tableBody.find(".norecordsfound").remove();
        }
        var vvalue = $(this).val().toLowerCase();
        if (vvalue == 0) {
            $("#OrgChartGrid tbody tr").show();
        }
    });
    $("#SearchOrgREmp").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#OrgChartGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#OrgChartGrid tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $tableBody.append(noRecordsRow);
        } else {
            $tableBody.find(".norecordsfound").remove();
        }
    });
    $("#NotAssDept").change(function () {
        if ($(this).is(":checked")) {
            var value = ("N").toLowerCase();
            $("#EmployeeGrid tbody tr").filter(function () {
                $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#EmployeeGrid tbody");
            if ($tableBody.find("tr:visible").length === 0) {
                const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
                $tableBody.append(noRecordsRow);
            } else {
                $tableBody.find(".norecordsfound").remove();
            }
        } else {
            $("#EmployeeGrid tbody tr").show();
        }
    });
    $("#RoleUnassigned").change(function () {
        if ($(this).is(":checked")) {
            var value = ("Y").toLowerCase();
            $("#EmployeeGrid tbody tr").filter(function () {
                $(this).toggle($(this.children[5]).text().toLowerCase().indexOf(value) > -1)
            });
            var $tableBody = $("#EmployeeGrid tbody");
            if ($tableBody.find("tr:visible").length === 0) {
                const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
                $tableBody.append(noRecordsRow);
            } else {
                $tableBody.find(".norecordsfound").remove();
            }
        } else {
            $("#EmployeeGrid tbody tr").show();
        }
    });

    $('#addOrgChart').on('hidden.bs.modal', function (event) {
        $("#OrgLoc").val(0);
        $("#OrgDept").val(0);
        $("#OrgRole").val(0);
        $("#OrgEmpl").val(0);
        $("#POrgId").val('');
        $("#OrgRoleReport").val(0);
        var OrgLoc = document.getElementById('OrgLoc');
        OrgLoc.style.border = '';
        var OrgDept = document.getElementById('OrgDept');
        OrgDept.style.border = '';
        var OrgRole = document.getElementById('OrgRole');
        OrgRole.style.border = '';
        var OrgEmpl = document.getElementById('OrgEmpl');
        OrgEmpl.style.border = '';
        var newNamevalidate = document.getElementById('OrgRoleReport');
        newNamevalidate.style.border = '';
        var UiAccessRMenu1 = $('#OrgDept');
        UiAccessRMenu1.html('');
        LoadOrgChart();

    });
    $('#addOrgChart').on('show.bs.modal', function (event) {
        var relatedTarget = $(event.relatedTarget);
        var orgid = relatedTarget.data("orgchartids");
        var toplevel = relatedTarget.data("toplevel");
        var roleid = relatedTarget.data("roleids");
        var dept = relatedTarget.data("dept");
        var reportto = relatedTarget.data("reportto");
        var employee = relatedTarget.data("employee");
        var plantid = relatedTarget.data("plantid");
        $("#OrgHead").prop("checked", false);
        //$("#OrgRole").prop("multiple", true);
        $("#DivRoleReportTo").show();
        if (orgid != undefined) {
            $("#OrgLoc").val(plantid).change(); // Set designation and trigger change
            //$("#OrgRole").prop("multiple", false);
            if (roleid.length > 1) {
                var roleidArray = roleid.split(",").map(value => value.trim());
                $("#OrgRole").val(roleidArray).change();
            } else {
                $("#OrgRole").val(roleid).change();
            }
            $("#OrgEmpl").val(employee).change();
            $("#OrgRoleReport").val(reportto).change();
            $("#POrgId").val(orgid);
            if (toplevel == "Y") {
                $("#OrgHead").prop("checked", true);
                $("#DivRoleReportTo").hide();
            } else {
                $("#OrgHead").prop("checked", false);
                $("#DivRoleReportTo").show();
            }
            var UiAccessRMenu1 = $('#OrgDept');
            UiAccessRMenu1.html('');
            const filteredData = Departments.filter(item => item.plantId === plantid);
            for (var i = 0; i < filteredData.length; i++) {
                div_data = "<option value='" + filteredData[i].departmentId + "'>" + filteredData[i].name + "</option>";
                UiAccessRMenu1.append(div_data);
            }
            $("#OrgDept").val(dept).change(); // Set department and trigger change
        }
    });
    $('#OrgHead').on('click', function () {
        if ($(this).is(':checked')) {
            $("#DivRoleReportTo").hide();
        } else {
            $("#DivRoleReportTo").show();
        }
    });
    const getrolesLevel = {
        "Plant Head": 1,
        "Admin": 1,
        "Operations": 2,
        "Marketing": 2,
        "Finance Head": 2,
        "Opr Head": 2,
        "Prodn Design Engineer": 2,
        "Prodn Design Head": 2,
        "Product Design": 2,
        "Manf Engg": 2,
        "Quality": 2,
        "Quality Head": 2,
        "Sales Admin": 2,
        "Dispatch": 2,
        "Maintenance": 2,
        "HR": 2,
        "Finance": 2,
        "PPC": 3,
        "Maintenance": 3,
        "Dispatch": 3,
        "Tool Store": 3,
        "Shift QA": 3,
        "Shift Supervisor": 3,
        "Operator": 4,
        "PD Engineer": 3,
        "MFE Engineer": 3,
        "MFE Head": 3,
        "Line Quality": 3,
        "Inward Quality": 3,
        "Inward Qlty": 3,
        "Final Quality": 3,
        "Final Qlty": 3,
        "Purchase": 3,
        "Matl Stores": 3,
        "Stores": 3
    };
    $("#OrgSave").click(function () {
        api.post("/Employee/SendEmail?to=test@example.com&subject=Hello&body=Hi").then((data) => {
            //LoadEmployee();
            //$("#POrgId").val('');
            //$("#OrgRoleReport").val(0);
            //$("#OrgLoc").val(0);
            //$("#OrgDept").val(0);
            //$("#OrgRole").val(0);
            //$("#OrgEmpl").val(0);
            //$("#addOrgChart").modal("hide");
            alert("Sent Successfully!");
        }).catch((error) => {
        });
        ////   //debugger;
        //var POrgId = $("#POrgId").val();
        //var OrgLoc = parseInt($("#OrgLoc").val());
        //var OrgDept = parseInt($("#OrgDept").val());
        //var OrgRole =  parseInt($("#OrgRole").val());
        //var OrgEmpl =  parseInt($("#OrgEmpl").val());
        //var OrgRoleReport =  parseInt($("#OrgRoleReport").val());
        //var checkbox = document.getElementById("OrgHead");
        //var EPResignChk = 'N';
        //const selectElement = document.getElementById('OrgRole');
        //const selectedValues = Array.from(selectElement.selectedOptions).map(option => Number(option.value));
        ////var selectedText = $("#OrgRole option:selected").text();
        //var roleidArray =[];
        //if (POrgId.length > 1) {
        //    roleidArray = POrgId.split(",").map(value => value.trim());
        //} else {
        //    roleidArray = POrgId;
        //}
        //if (checkbox.checked) {
        //    EPResignChk = 'Y';
        //}
        //if (OrgLoc=== 0) {
        //    var newNamevalidate = document.getElementById('OrgLoc');
        //    newNamevalidate.style.border = '2px solid red';
        //    return false;
        //} else {
        //    var newNamevalidate = document.getElementById('OrgLoc');
        //    newNamevalidate.style.border = '';
        //}
        //if (OrgDept=== 0) {
        //    var newNamevalidate = document.getElementById('OrgDept');
        //    newNamevalidate.style.border = '2px solid red';
        //    return false;
        //} else {
        //    var newNamevalidate = document.getElementById('OrgDept');
        //    newNamevalidate.style.border = '';
        //}
        //if (OrgRole === 0 || isNaN(OrgRole)) {
        //    var newNamevalidate = $('#OrgRole').next('.select2-container');
        //    newNamevalidate.css('border', '2px solid red');
        //    return false;
        //} else {
        //    var newNamevalidate = $('#OrgRole').next('.select2-container');
        //    newNamevalidate.css('border', '');
        //}
        ////if (OrgEmpl=== 0) {
        ////    var newNamevalidate = document.getElementById('OrgEmpl');
        ////    newNamevalidate.style.border = '2px solid red';
        ////    return false;
        ////} else {
        ////    var newNamevalidate = document.getElementById('OrgEmpl');
        ////    newNamevalidate.style.border = '';
        ////}
        ////if (EPResignChk === "N") {
        ////    if (OrgRoleReport === 0) {
        ////        var newNamevalidate = document.getElementById('OrgRoleReport');
        ////        newNamevalidate.style.border = '2px solid red';
        ////        return false;
        ////    } else {
        ////        var newNamevalidate = document.getElementById('OrgRoleReport');
        ////        newNamevalidate.style.border = '';
        ////    }
        ////} else {
        ////}
        //api.get("/Employee/GetOrgChart").then((data) => {
        //    const filteredData = data.filter(item => item.first_node === "Y");
        //    if (!filteredData.some(item => item.first_node === EPResignChk)) {
        //        const selectElementText = document.getElementById('OrgRole');
        //        const selectedValuesText = Array.from(selectElementText.selectedOptions);
        //        for (var i = 0; i < selectedValues.length; i++) {
        //            var selectedText = selectedValuesText[i].textContent;
        //            var level = getrolesLevel[selectedText];
        //            var orgids = roleidArray[i];
        //            var rowData = {
        //                org_ChartId: orgids,
        //                first_node: EPResignChk,
        //                role_NameId: selectedValues[i],
        //                dept_ID: OrgDept,
        //                location_id: OrgLoc,
        //                reporting_to: OrgRoleReport,
        //                employee_Id: OrgEmpl,
        //                level_No: parseInt(level)
        //            };
        //            api.post("/Employee/PostOrgChart", rowData).then((data) => {
        //                //LoadEmployee();
        //                //$("#POrgId").val('');
        //                //$("#OrgRoleReport").val(0);
        //                //$("#OrgLoc").val(0);
        //                //$("#OrgDept").val(0);
        //                //$("#OrgRole").val(0);
        //                //$("#OrgEmpl").val(0);
        //                //$("#addOrgChart").modal("hide");
        //                if (i === selectedValues.length - 1) {
        //                    // loadSelectRole();
        //                    $("#addOrgChart").modal("hide");
        //                }
        //                alert("Org Chart Position Details Saved Successfully!");
        //            }).catch((error) => {
        //            });
        //        }
        //        if (selectedValues.length < roleidArray.length) {
        //            var j = roleidArray.length - 1;
        //            var opid = roleidArray[j];
        //            api.get("/Employee/DelOrgChart?designationId=" + parseInt(opid)).then((data) => {
        //                //LoadOrgChart();
        //            }).catch((error) => {

        //            });
        //        }
        //    } else {
        //        alert("The Head Of Organization Is Already There In The List.");
        //    }
        //}).catch((error) => {

        //});

    });

    $('#uiList').on('show.bs.modal', function (event) {
        loadUiList();
        loadSelectMenus();
    });
    $('#UPTopLevel').on('click', function () {
        if ($(this).is(':checked')) {
            $("#DivUiPartOf").hide();
        } else {
            $("#DivUiPartOf").show();
        }
    });
    $('#addUi').on('hidden.bs.modal', function (event) {
        document.getElementById('uiList').style.filter = 'none';
        $("#DivUiPartOf").show();
        var newNamevalidate = document.getElementById('UPUiName');
        newNamevalidate.style.border = '';
        var UPPartOf = document.getElementById('UPPartOf');
        UPPartOf.style.border = '';
        $("#UPUiName").val('');
        $("#UPUiListId").val('');
        $("#UPTopLevel").prop("checked", false);
        $("#UPView").prop("checked", false);
        $("#UPAddEdit").prop("checked", false);
        $("#UPDelete").prop("checked", false);
        $("#UPApprove").prop("checked", false);
        $("#UPUiType").val("Landing Page").change();
    });
    $('#addUi').on('show.bs.modal', function (event) {
        document.getElementById('uiList').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var uilistid = relatedTarget.data("uilistid");
        var toplevel = relatedTarget.data("toplevel");
        var uitype = relatedTarget.data("uitype");
        var uiname = relatedTarget.data("uiname");
        var uipartto = relatedTarget.data("uipartto");
        var approveallow = relatedTarget.data("approveallow");
        var viewallow = relatedTarget.data("viewallow");
        var addedit = relatedTarget.data("addedit");
        var deleteallow = relatedTarget.data("deleteallow");
        if (uilistid > 0) {
            if (toplevel == "Y") {
                $("#UPTopLevel").prop("checked", true);
                $("#DivUiPartOf").hide();
            } else {
                $("#UPTopLevel").prop("checked", false);
                $("#UPPartOf").val(uipartto).change();
                $("#DivUiPartOf").show();
            }
            if (viewallow == "Y") {
                $("#UPView").prop("checked", true);
            } else {
                $("#UPView").prop("checked", false);
            }
            if (addedit == "Y") {
                $("#UPAddEdit").prop("checked", true);
            } else {
                $("#UPAddEdit").prop("checked", false);
            }
            if (deleteallow == "Y") {
                $("#UPDelete").prop("checked", true);
            } else {
                $("#UPDelete").prop("checked", false);
            }
            if (approveallow == "Y") {
                $("#UPApprove").prop("checked", true);
            } else {
                $("#UPApprove").prop("checked", false);
            }
            $("#UPUiType").val(uitype).change(); 
            $("#UPUiName").val(uiname);
            $("#UPUiListId").val(uilistid);
        }
    });
    $("#UPSaveUi").click(function () {
        var UPUiName = $("#UPUiName").val();
        var UPUiType = $("#UPUiType").val();
        var UPUiListId = $("#UPUiListId").val();
        var UPPartOf = parseInt($("#UPPartOf").val());
        var OrgRoleReport = parseInt($("#OrgRoleReport").val());
        var checkbox = document.getElementById("UPTopLevel");
        if (UPUiName.length <= 0) {
            var newNamevalidate = document.getElementById('UPUiName');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('UPUiName');
            newNamevalidate.style.border = '';
        }
        var EPResignChk = 'N';
        if (checkbox.checked) {
            EPResignChk = 'Y';
        } else {
            if (UPPartOf == 0) {
                var newNamevalidate = document.getElementById('UPPartOf');
                newNamevalidate.style.border = '2px solid red';
                return false;
            } else {
                var newNamevalidate = document.getElementById('UPPartOf');
                newNamevalidate.style.border = '';
            }}
        var UPView = document.getElementById("UPView");
        var UPViewChk = 'N';
        if (UPView.checked) {
            UPViewChk = 'Y';
        }
        var UPAddEdit = document.getElementById("UPAddEdit");
        var UPAddEditChk = 'N';
        if (UPAddEdit.checked) {
            UPAddEditChk = 'Y';
        }
        var UPDelete = document.getElementById("UPDelete");
        var UPDeleteChk = 'N';
        if (UPDelete.checked) {
            UPDeleteChk = 'Y';
        }
        var UPApprove = document.getElementById("UPApprove");
        var UPApproveChk = 'N';
        if (UPApprove.checked) {
            UPApproveChk = 'Y';
        }
        var rowData = {
            uiListId: UPUiListId,
            TopLevelId: EPResignChk,
            ui_Type: UPUiType,
            ui_Name_Label: UPUiName,
            ui_Part_linked_to: UPPartOf,
            approval_Allowed: UPApproveChk,
            view_Allowed: UPViewChk,
            add_Edit_Allowed: UPAddEditChk,
            delete_Allowed: UPDeleteChk
        };

        //api.getbulk("/Employee/GetUniqueUiName?uiName=" + UPUiName).then((data) => {
        //    if (data == true || UPUiListId >0) {
        //        $("#error-uiname").text("").css("color", "red");
                api.post("/Employee/PostUilist", rowData).then((data) => {
                    loadUiList();
                    alert("UI Details Saved Successfully!");
                }).catch((error) => {
                    //AppUtil.HandleError("frmDesignation", error);
                });
        //    } else {
        //        $("#error-uiname").text("Please enter a different Ui Name.").css("color", "red");
        //    }
        //    //console.log(tablebody);
        //}).catch((error) => { });
    });

    $('#roleList').on('show.bs.modal', function (event) {
        LoadRoleUiAll();
    });
    $("#SearchRlRoleName").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#RoleGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#RoleGrid tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $tableBody.append(noRecordsRow);
        } else {
            $tableBody.find(".norecordsfound").remove();
        }
    });
    $("#SearchRlUiAccess").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#RoleGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#RoleGrid tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $tableBody.append(noRecordsRow);
        } else {
            $tableBody.find(".norecordsfound").remove();
        }
    });
    $("#SearchUim1").on("change", function () {
        var value = $(this).find("option:selected").text().toLowerCase();
        $("#UiGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[0]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#UiGrid tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $tableBody.append(noRecordsRow);
        } else {
            $tableBody.find(".norecordsfound").remove();
        }
        var UiAccessRMenu1 = $('#SearchUim2');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);

        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        for (var i = 0; i < menusdata.length; i++) {
            if (menusdata[i].uI_Part_linked_to == formattedSelectedDate) {
                div_data = "<option value='" + menusdata[i].uiListId + "'>" + menusdata[i].menu2 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
        }
        var vvalue = $(this).val().toLowerCase();
        if (vvalue == 0) {
            $("#UiGrid tbody tr").show();
            var SearchUim2 = $('#SearchUim2');
            SearchUim2.html('');
            var SearchUim3 = $('#SearchUim3');
            SearchUim3.html('');
            var SearchUim4 = $('#SearchUim4');
            SearchUim4.html('');
            var SearchUim5 = $('#SearchUim5');
            SearchUim5.html('');

        }
    });
    $("#SearchUim2").on("change", function () {
        var value = $(this).find("option:selected").text().toLowerCase();
        $("#UiGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[1]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#UiGrid tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $tableBody.append(noRecordsRow);
        } else {
            $tableBody.find(".norecordsfound").remove();
        }
        var UiAccessRMenu1 = $('#SearchUim3');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);

        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        for (var i = 0; i < menusdata.length; i++) {
            if (menusdata[i].uI_Part_linked_to == formattedSelectedDate) {
                div_data = "<option value='" + menusdata[i].uiListId + "'>" + menusdata[i].menu3 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
        }
        var vvalue = $(this).val().toLowerCase();
        if (vvalue == 0) {
            $("#UiGrid tbody tr").show();
            var SearchUim3 = $('#SearchUim3');
            SearchUim3.html('');
            var SearchUim4 = $('#SearchUim4');
            SearchUim4.html('');
            var SearchUim5 = $('#SearchUim5');
            SearchUim5.html('');
        }
    });
    $("#SearchUim3").on("change", function () {
        var value = $(this).find("option:selected").text().toLowerCase();
        $("#UiGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[2]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#UiGrid tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $tableBody.append(noRecordsRow);
        } else {
            $tableBody.find(".norecordsfound").remove();
        }
        var UiAccessRMenu1 = $('#SearchUim4');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);

        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        for (var i = 0; i < menusdata.length; i++) {
            if (menusdata[i].uI_Part_linked_to == formattedSelectedDate) {
                div_data = "<option value='" + menusdata[i].uiListId + "'>" + menusdata[i].menu4 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
        }
        var vvalue = $(this).val().toLowerCase();
        if (vvalue == 0) {
            $("#UiGrid tbody tr").show();
            var SearchUim4 = $('#SearchUim4');
            SearchUim4.html('');
            var SearchUim5 = $('#SearchUim5');
            SearchUim5.html('');
        }
    });
    $("#SearchUim4").on("change", function () {
        var value = $(this).find("option:selected").text().toLowerCase();
        $("#UiGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[3]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#UiGrid tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $tableBody.append(noRecordsRow);
        } else {
            $tableBody.find(".norecordsfound").remove();
        }
        var UiAccessRMenu1 = $('#SearchUim5');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);

        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        for (var i = 0; i < menusdata.length; i++) {
            if (menusdata[i].uI_Part_linked_to == formattedSelectedDate) {
                div_data = "<option value='" + menusdata[i].uiListId + "'>" + menusdata[i].menu5 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
        }
        var vvalue = $(this).val().toLowerCase();
        if (vvalue == 0) {
            $("#UiGrid tbody tr").show();
            var SearchUim5 = $('#SearchUim5');
            SearchUim5.html('');
        }
    });
    $("#SearchUim5").on("change", function () {
        var value = $(this).find("option:selected").text().toLowerCase();
        $("#UiGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[4]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#UiGrid tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $tableBody.append(noRecordsRow);
        } else {
            $tableBody.find(".norecordsfound").remove();
        }
        var vvalue = $(this).val().toLowerCase();
        if (vvalue == 0) {
            $("#UiGrid tbody tr").show();
        }
    });
    $("#SearchUiType").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#UiGrid tbody tr").filter(function () {
            $(this).toggle($(this.children[5]).text().toLowerCase().indexOf(value) > -1)
        });
        var $tableBody = $("#UiGrid tbody");
        if ($tableBody.find("tr:visible").length === 0) {
            const noRecordsRow = `
                <tr class="norecordsfound">
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $tableBody.append(noRecordsRow);
        } else {
            $tableBody.find(".norecordsfound").remove();
        }
    });
    $('#addRole').on('hidden.bs.modal', function (event) {
        ////document.getElementById('roleList').style.filter = 'none';
        var newNamevalidate = document.getElementById('ARPWork');
        newNamevalidate.style.border = '';
        var ARPName = document.getElementById('ARPName');
        ARPName.style.border = '';
    });
    $('#addRole').on('show.bs.modal', function (event) {
        //document.getElementById('roleList').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var roleid = relatedTarget.data("roleid");
        var rolename = relatedTarget.data("rolename");
        var uitype = relatedTarget.data("workdone");
        $("#ARPId").val(roleid);
        $("#ARPName").val(rolename);
        $("#ARPWork").val(uitype);
        loadSelectMenus();
        LoadRoleUiById(roleid);
    });
    $('#addUiAccessRole').on('hidden.bs.modal', function (event) {
        document.getElementById('addRole').style.filter = 'none';
        var UiAccessRMenu1 = document.getElementById('UiAccessRMenu1');
        UiAccessRMenu1.style.border = '';
        var UiAccessRMenu2 = document.getElementById('UiAccessRMenu2');
        UiAccessRMenu2.style.border = '';
        var UiAccessRMenu3 = document.getElementById('UiAccessRMenu3');
        UiAccessRMenu3.style.border = '';
        var UiAccessRMenu4 = document.getElementById('UiAccessRMenu4');
        UiAccessRMenu4.style.border = '';
        var UiAccessRMenu5 = document.getElementById('UiAccessRMenu5');
        UiAccessRMenu5.style.border = '';
        var UiAccessRPermission = document.getElementById('UiAccessRPermission');
        UiAccessRPermission.style.border = '';
        $("#UiAccessRMenu1").val(0);
        $("#UiAccessRMenu2").val(0);
        $("#UiAccessRMenu3").val(0);
        $("#UiAccessRMenu4").val(0);
        $("#UiAccessRMenu5").val(0);
        $("#UiAccessREmplid").val(0);
        $("#UiAccessRPermission").val(0);
    });
    $('#addUiAccessRole').on('show.bs.modal', function (event) {
        document.getElementById('addRole').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var uilistid = relatedTarget.data("uilistid");
        var roleid = relatedTarget.data("roleid");
        var uid = relatedTarget.data("uid");
        var permissionid = relatedTarget.data("permissionid");
        var uilevel = relatedTarget.data("uilevel");
        var menuo = relatedTarget.data("menuo");
        var menut = relatedTarget.data("menut");
        var menuth = relatedTarget.data("menuth");
        var menuf = relatedTarget.data("menuf");
        var menufi = relatedTarget.data("menufi");
        var roleidnew = $("#ARPId").val();
        var ARPName = $("#ARPName").val();
        $("#SRoleNameUI").text(ARPName);
        $("#UiAccessRRoleid").val(roleidnew);
        if (uilistid > 0) {
            //const menuSelections = {
            //    1: { menu1: 1, menu2: 0, menu3: 0, menu4: 0, menu5: 0, permission: 0 },
            //    2: { menu1: 1, menu2: 2, menu3: 0, menu4: 0, menu5: 0, permission: 0 },
            //    3: { menu1: 1, menu2: 2, menu3: 3, menu4: 0, menu5: 0, permission: 0 },
            //    4: { menu1: 1, menu2: 2, menu3: 3, menu4: 4, menu5: 0, permission: 0 },
            //    5: { menu1: 1, menu2: 2, menu3: 3, menu4: 4, menu5: 6, permission: 0 },
            //    6: { menu1: 1, menu2: 2, menu3: 3, menu4: 4, menu5: 6, permission: 0 },
            //    // Add more mappings as needed
            //};

            //const selection = menuSelections[uid];
            $('#UiAccessRMenu1 option').each(function () {
                if ($(this).text() === menuo) {
                    $('#UiAccessRMenu1').val($(this).val()).change();
                }
            });
            $('#UiAccessRMenu2 option').each(function () {
                if ($(this).text() === menut) {
                    $('#UiAccessRMenu2').val($(this).val()).change();
                }
            });
            $('#UiAccessRMenu3 option').each(function () {
                if ($(this).text() === menuth) {
                    $('#UiAccessRMenu3').val($(this).val()).change();
                }
            });
            $('#UiAccessRMenu4 option').each(function () {
                if ($(this).text() === menuf) {
                    $('#UiAccessRMenu4').val($(this).val()).change();
                }
            });
            $('#UiAccessRMenu5 option').each(function () {
                if ($(this).text() === menufi) {
                    $('#UiAccessRMenu5').val($(this).val()).change();
                }
            });
            $("#UiAccessRPermission").val(permissionid).change();
            $("#UiAccessRRoleid").val(roleid);
            $("#UiAccessRUiId").val(uilistid);
        }
    });
    $("#UiAccessRSave").secureClick(function () {
        var UiAccessRMenu1 = parseInt($("#UiAccessRMenu1").val());
        var UiAccessRMenu2 = parseInt($("#UiAccessRMenu2").val());
        var UiAccessRMenu3 = parseInt($("#UiAccessRMenu3").val());
        var UiAccessRMenu4 = parseInt($("#UiAccessRMenu4").val());
        var UiAccessRMenu5 = parseInt($("#UiAccessRMenu5").val());
        var UiAccessRRoleid = parseInt($("#UiAccessRRoleid").val());
        var UiAccessRUiId = parseInt($("#UiAccessRUiId").val());
        var UiAccessRPermission = parseInt($("#UiAccessRPermission").val());
        var ARPName = $("#ARPName").val();
        var ARPId = $("#ARPId").val();
        if (UiAccessRRoleid == 0 || isNaN(UiAccessRRoleid)) {
            alert("Please Save the Role Name");
            return false;
        }
        let uiId = 0;
        if (UiAccessRMenu5 != 0) {
            uiId = UiAccessRMenu5;
        } else if (UiAccessRMenu4 != 0) {
            uiId = UiAccessRMenu4;
        } else if (UiAccessRMenu3 != 0) {
            uiId = UiAccessRMenu3;
        } else if (UiAccessRMenu2 != 0) {
            uiId = UiAccessRMenu2;
        } else {
            uiId = UiAccessRMenu1;  // Default case when all above are zero
        }

        if (UiAccessRMenu1 == 0) {
            var newvalidate = document.getElementById('UiAccessRMenu1');
            newvalidate.style.border = '2px solid red';
            return false;
        } else {
            var newvalidate = document.getElementById('UiAccessRMenu1');
            newvalidate.style.border = '';
        }
        //if (UiAccessRMenu2 == 0) {
        //    var newvalidate = document.getElementById('UiAccessRMenu2');
        //    newvalidate.style.border = '2px solid red';
        //    return false;
        //} else {
        //    var newvalidate = document.getElementById('UiAccessRMenu2');
        //    newvalidate.style.border = '';
        //}
        //if (UiAccessRMenu3 == 0) {
        //    var newvalidate = document.getElementById('UiAccessRMenu3');
        //    newvalidate.style.border = '2px solid red';
        //    return false;
        //} else {
        //    var newvalidate = document.getElementById('UiAccessRMenu3');
        //    newvalidate.style.border = '';
        //}
        //if (UiAccessRMenu4 == 0) {
        //    var newvalidate = document.getElementById('UiAccessRMenu4');
        //    newvalidate.style.border = '2px solid red';
        //    return false;
        //} else {
        //    var newvalidate = document.getElementById('UiAccessRMenu4');
        //    newvalidate.style.border = '';
        //}
        //if (UiAccessRMenu5 == 0) {
        //    var newvalidate = document.getElementById('UiAccessRMenu5');
        //    newvalidate.style.border = '2px solid red';
        //    return false;
        //} else {
        //    var newvalidate = document.getElementById('UiAccessRMenu5');
        //    newvalidate.style.border = '';
        //}
        if (UiAccessRPermission == 0) {
            var newvalidate = document.getElementById('UiAccessRPermission');
            newvalidate.style.border = '2px solid red';
            return false;
        } else {
            var newvalidate = document.getElementById('UiAccessRPermission');
            newvalidate.style.border = '';
        }
        const menuIds = [
            UiAccessRMenu1,
            UiAccessRMenu2,
            UiAccessRMenu3,
            UiAccessRMenu4,
            UiAccessRMenu5
        ].filter(id => id && id !== 0);
        const uiIdString = menuIds.join(",");

        var rowData = {
            role_Ui_ListId: UiAccessRUiId,
            ui_Id: uiIdString,
            permissionId: UiAccessRPermission,
            roleId: UiAccessRRoleid
        };

        //api.getbulk("/Employee/GetUniqueRole?roleName=" + ARPName).then((data) => {
        //    if (data == true) {
        //        $("#error-rolename").text("").css("color", "red");
        return api.post("/Employee/PostRoleUiList", rowData).then((data) => {
            LoadRoleUiById(UiAccessRRoleid);
            LoadRoleUiAll();
            $("#addUiAccessRole").modal("hide");
            alert("UI Access Saved Successfully!");
                    //LoadEmployee();
                    //loadSelectRole();
                }).catch((error) => {
                    //AppUtil.HandleError("frmDesignation", error);
                });
        //    } else {
        //        $("#error-rolename").text("Please enter a different Empl No.").css("color", "red");
        //    }
        //    //console.log(tablebody);
        //}).catch((error) => { });
    });

    $("#ARPSave").secureClick(function () {
        var ARPWork = $("#ARPWork").val();
        var ARPName = $("#ARPName").val();
        var ARPId = $("#ARPId").val();
        if (ARPName.length <= 0) {
            var newNamevalidate = document.getElementById('ARPName');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('ARPName');
            newNamevalidate.style.border = '';
        }
        if (ARPWork.length <= 0) {
            var newNamevalidate = document.getElementById('ARPWork');
            newNamevalidate.style.border = '2px solid red';
            return false;
        } else {
            var newNamevalidate = document.getElementById('ARPWork');
            newNamevalidate.style.border = '';
        }
        var rowData = {
            role_Desc: ARPName,
            work_Done: ARPWork,
            role_ListId: ARPId
        };

        return api.getbulk("/Employee/GetUniqueRole?roleName=" + ARPName).then((data) => {
            if (data == true || ARPId > 0) {
                $("#error-rolename").text("").css("color", "red");
                api.post("/Employee/PostRolelist", rowData).then((data) => {
                    //LoadEmployee();
                    $("#ARPId").val(data.role_ListId);
                    $("#UiAccessRRoleid").val(data.role_ListId);
                    loadSelectRole();
                    LoadRoleUiAll();
                    LoadRoleUiById(data.role_ListId);
                    alert("Role Saved Successfully!");
                }).catch((error) => {
                    //AppUtil.HandleError("frmDesignation", error);
                });
            } else {
                $("#error-rolename").text("This Role Name Already Exists.").css("color", "red");
            }
            //console.log(tablebody);
        }).catch((error) => { });
    });


    $('#addUiAccessEmpl').on('hidden.bs.modal', function (event) {
        document.getElementById('addEmployee').style.filter = 'none';
        var UiAccessRMenu1 = document.getElementById('UiAccessEMenu1');
        UiAccessRMenu1.style.border = '';
        var UiAccessRMenu2 = document.getElementById('UiAccessEMenu2');
        UiAccessRMenu2.style.border = '';
        var UiAccessRMenu3 = document.getElementById('UiAccessEMenu3');
        UiAccessRMenu3.style.border = '';
        var UiAccessRMenu4 = document.getElementById('UiAccessEMenu4');
        UiAccessRMenu4.style.border = '';
        var UiAccessRMenu5 = document.getElementById('UiAccessEMenu5');
        UiAccessRMenu5.style.border = '';
        var UiAccessRPermission = document.getElementById('UiAccessEPermission');
        UiAccessRPermission.style.border = '';
        $("#UiAccessEEmplid").val(0)
        $("#UiAccessEUiId").val(''); 
        $("#UiAccessEMenu1").val(0);
        $("#UiAccessEMenu2").val(0);
        $("#UiAccessEMenu3").val(0);
        $("#UiAccessEMenu4").val(0);
        $("#UiAccessEMenu5").val(0);
        $("#UiAccessEEmplid").val(0);
        $("#UiAccessEPermission").val(0);
    });
    $('#addUiAccessEmpl').on('show.bs.modal', function (event) {
        document.getElementById('addEmployee').style.filter = 'blur(5px)';
        var relatedTarget = $(event.relatedTarget);
        var uilistid = relatedTarget.data("uilistid");
        var roleid = relatedTarget.data("roleid");
        var uid = relatedTarget.data("uid");
        var permissionid = relatedTarget.data("permissionid");
        var uilevel = relatedTarget.data("uilevel");
        var menuo = relatedTarget.data("menuo");
        var menut = relatedTarget.data("menut");
        var menuth = relatedTarget.data("menuth");
        var menuf = relatedTarget.data("menuf");
        var menufi = relatedTarget.data("menufi");
        var empid = $("#EPEmpId").val();
        var EPEmpName = $("#EPEmpName").val();
        $("#SEmpNameUI").text(EPEmpName);
        $("#UiAccessEEmplid").val(empid);
        if (uilistid > 0) {
            $('#UiAccessEMenu1 option').each(function () {
                if ($(this).text() === menuo) {
                    $('#UiAccessEMenu1').val($(this).val()).change();
                }
            });
            $('#UiAccessEMenu2 option').each(function () {
                if ($(this).text() === menut) {
                    $('#UiAccessEMenu2').val($(this).val()).change();
                }
            });
            $('#UiAccessEMenu3 option').each(function () {
                if ($(this).text() === menuth) {
                    $('#UiAccessEMenu3').val($(this).val()).change();
                }
            });
            $('#UiAccessEMenu4 option').each(function () {
                if ($(this).text() === menuf) {
                    $('#UiAccessEMenu4').val($(this).val()).change();
                }
            });
            $('#UiAccessEMenu5 option').each(function () {
                if ($(this).text() === menufi) {
                    $('#UiAccessEMenu5').val($(this).val()).change();
                }
            });
            $("#UiAccessEPermission").val(permissionid).change();
            $("#UiAccessERoleId").val(roleid);
            $("#UiAccessEUiId").val(uilistid);
        }
    });
    $("#UiAccessESave").secureClick(function () {
        var UiAccessRMenu1 = parseInt($("#UiAccessEMenu1").val());
        var UiAccessRMenu2 = parseInt($("#UiAccessEMenu2").val());
        var UiAccessRMenu3 = parseInt($("#UiAccessEMenu3").val());
        var UiAccessRMenu4 = parseInt($("#UiAccessEMenu4").val());
        var UiAccessRMenu5 = parseInt($("#UiAccessEMenu5").val());
        var UiAccessRRoleid =$("#UiAccessEEmplid").val();
        var UiAccessRUiId = parseInt($("#UiAccessEUiId").val());
        var UiAccessERoleId = parseInt($("#UiAccessERoleId").val());
        var UiAccessRPermission = parseInt($("#UiAccessEPermission").val());
        var deptid = $("#EPDept").val();
        //var ARPId = $("#ARPId").val();
        if (UiAccessRRoleid == 0) {
            alert("Please Save the Employee Details");
            return false;
        }
        if (UiAccessRMenu1 == 0) {
            var newvalidate = document.getElementById('UiAccessEMenu1');
            newvalidate.style.border = '2px solid red';
            return false;
        } else {
            var newvalidate = document.getElementById('UiAccessEMenu1');
            newvalidate.style.border = '';
        }
        if (UiAccessRPermission == 0) {
            var newvalidate = document.getElementById('UiAccessEPermission');
            newvalidate.style.border = '2px solid red';
            return false;
        } else {
            var newvalidate = document.getElementById('UiAccessEPermission');
            newvalidate.style.border = '';
        }
        var todaydate = new Date().toISOString().slice(0, 19);

        const menuIds = [
            UiAccessRMenu1,
            UiAccessRMenu2,
            UiAccessRMenu3,
            UiAccessRMenu4,
            UiAccessRMenu5
        ].filter(id => id && id !== 0);
        const uiIdString = menuIds.join(",");

        var rowData = {
            employee_UI_ListId: UiAccessRUiId,
            ui_Id: uiIdString,
            access_Level: UiAccessRPermission,
            employee_Id: UiAccessRRoleid,
            active: 'Y',
            add_date: todaydate
        };
        return api.post("/Department/PostEmployee_UI_List", rowData).then((data) => {
            LoadEmplUiById();
            $("#addUiAccessEmpl").modal("hide");
        }).catch((error) => {
            //AppUtil.HandleError("frmDesignation", error);
        });
    });

    //loadSelectRole();
    loadSelectPermission();


    $("#UiAccessRMenu1").on("change", function () {
        var UiAccessRMenu1 = $('#UiAccessRMenu2');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);

        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        for (var i = 0; i < menusdata.length; i++) {
            if (menusdata[i].uI_Part_linked_to == formattedSelectedDate) {
                div_data = "<option value='" + menusdata[i].uiListId + "'>" + menusdata[i].menu2 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
        }
    });
    $("#UiAccessRMenu2").on("change", function () {
        var UiAccessRMenu1 = $('#UiAccessRMenu3');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);

        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        for (var i = 0; i < menusdata.length; i++) {
            if (menusdata[i].uI_Part_linked_to == formattedSelectedDate) {
                div_data = "<option value='" + menusdata[i].uiListId + "'>" + menusdata[i].menu3 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
        }
    });
    $("#UiAccessRMenu3").on("change", function () {
        var UiAccessRMenu1 = $('#UiAccessRMenu4');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);

        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        for (var i = 0; i < menusdata.length; i++) {
            if (menusdata[i].uI_Part_linked_to == formattedSelectedDate) {
                div_data = "<option value='" + menusdata[i].uiListId + "'>" + menusdata[i].menu4 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
        }
    });
    $("#UiAccessRMenu4").on("change", function () {
        var UiAccessRMenu1 = $('#UiAccessRMenu5');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);

        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        for (var i = 0; i < menusdata.length; i++) {
            if (menusdata[i].uI_Part_linked_to == formattedSelectedDate) {
                div_data = "<option value='" + menusdata[i].uiListId + "'>" + menusdata[i].menu5 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
        }
    });
    $("#UiAccessEMenu1").on("change", function () {
        var UiAccessRMenu1 = $('#UiAccessEMenu2');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);

        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        for (var i = 0; i < menusdataEmpl.length; i++) {
            if (menusdataEmpl[i].uI_Part_linked_to == formattedSelectedDate) {
                div_data = "<option value='" + menusdataEmpl[i].uiListId + "'>" + menusdataEmpl[i].menu2 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
        }
    });
    $("#UiAccessEMenu2").on("change", function () {
        var UiAccessRMenu1 = $('#UiAccessEMenu3');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);

        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        for (var i = 0; i < menusdataEmpl.length; i++) {
            if (menusdataEmpl[i].uI_Part_linked_to == formattedSelectedDate) {
                div_data = "<option value='" + menusdataEmpl[i].uiListId + "'>" + menusdataEmpl[i].menu3 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
        }
    });
    $("#UiAccessEMenu3").on("change", function () {
        var UiAccessRMenu1 = $('#UiAccessEMenu4');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);

        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        for (var i = 0; i < menusdataEmpl.length; i++) {
            if (menusdataEmpl[i].uI_Part_linked_to == formattedSelectedDate) {
                div_data = "<option value='" + menusdataEmpl[i].uiListId + "'>" + menusdataEmpl[i].menu4 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
        }
    });
    $("#UiAccessEMenu4").on("change", function () {
        var UiAccessRMenu1 = $('#UiAccessEMenu5');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);

        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        for (var i = 0; i < menusdataEmpl.length; i++) {
            if (menusdataEmpl[i].uI_Part_linked_to == formattedSelectedDate) {
                div_data = "<option value='" + menusdataEmpl[i].uiListId + "'>" + menusdataEmpl[i].menu5 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
        }
    });
    $("#EPloc").on("change", function () {
        var UiAccessRMenu1 = $('#EPDept');
        UiAccessRMenu1.html('');
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);
        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        const filteredData = Departments.filter(item => item.plantId === formattedSelectedDate);
        for (var i = 0; i < filteredData.length; i++) {
            div_data = "<option value='" + filteredData[i].departmentId + "'>" + filteredData[i].name + "</option>";
                UiAccessRMenu1.append(div_data);
        }
    });
    $("#OrgLoc").on("change", function () {
        var UiAccessRMenu1 = $('#OrgDept');
        UiAccessRMenu1.html('');
        //div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        //UiAccessRMenu1.append(div_data);
        var selectedDate = $(this).val();
        var formattedSelectedDate = parseInt(selectedDate);
        const filteredData = Departments.filter(item => item.plantId === formattedSelectedDate);
        for (var i = 0; i < filteredData.length; i++) {
            div_data = "<option value='" + filteredData[i].departmentId + "'>" + filteredData[i].name + "</option>";
                UiAccessRMenu1.append(div_data);
        }
    });
    $('#Popup1').on('hidden.bs.modal', function (event) {
        document.getElementById('addEmployee').style.filter = 'none';
    });
    $('#Popup1').on('show.bs.modal', function (event) {
        document.getElementById('addEmployee').style.filter = 'blur(5px)';
        $("#Slvl2").val(0);
        $("#Slvl3").val(0);
        $("#Slvl4").val(0);
        $("#Slvl5").val(0);
        LoadDepartments();
    });
    $(document).on("change", ".row-checkbox", function () {
        $(".row-checkbox").not(this).prop("checked", false); // uncheck others
    });
    $("#DeptLinkSave").secureClick( function () {
        var selectedRow = $("#DeptP1Grid tbody tr").has("input.row-checkbox:checked");

        if (selectedRow.length === 0) {
            alert("Please select one record only.");
            return;
        }
        var empId = $("#EPEmpId").val();
        if (empId == 0) {
            alert("Please Save the Employee Details");
            return false;
        }
        if (isNaN(empId)) {
            alert("Please Save the Employee Details");
            return false;
        }
        // get hidden td value (last column)
        var hiddenId = selectedRow.find("td:hidden").text().trim();
        console.log("Selected Id:", hiddenId);
        var todaydate = new Date().toISOString().slice(0, 19);
        var rowData = {
            dept_Posn: hiddenId,
            employee_Id: parseInt(empId),
            active: 'Y',
            add_date: todaydate
        };
        return api.post("/Department/PostDept_Employee", rowData).then((data) => {
            $("#Popup1").modal("hide");
            LoadDeptEmp(parseInt(empId));
            LoadDepartments();
            LoadEmplUiById();
        }).catch((error) => {

        });
    });
    $(document).on("change", "#DectUi", function () {
        var empId = parseInt($("#EPEmpId").val());
        $("#ReportToDept").val('');
        if ($(this).is(":checked")) {
            var tablebody = $("#EmpDeptLinkGrid tbody");
            $(tablebody).html("");//empty tbody
            api.get("/department/GetDept_Employee").then((data) => {
                //console.log(data);
                data = data.filter(i => i.level2 != "-" && i.employee_Id === empId && i.active == "N");
                if (data.length === 0) {
                    // 2. Insert the "No Records Found" row
                    // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
                    const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
                    $(tablebody).append(noRecordsRow);
                }
                for (i = 0; i < data.length; i++) {
                    var previousval = $("#ReportToDept").val();
                    data[i].showMenu ="none";
                    $(tablebody).append(AppUtil.ProcessTemplateDataNew("EmpDeptLinkGridRow", data[i], i));
                    let lastNode = null;
                    let parentNode = null;
                    let deptId = 0;

                    if (data[i].level5 && data[i].level5 !== "-") {
                        lastNode = "Level5";
                        parentNode = data[i].level4 !== "-" ? data[i].level4 : null;
                        deptId = data[i].dept_Posn;
                    } else if (data[i].level4 && data[i].level4 !== "-") {
                        lastNode = "Level4";
                        parentNode = data[i].level3 !== "-" ? data[i].level3 : null;
                        deptId = data[i].dept_Posn;
                    } else if (data[i].level3 && data[i].level3 !== "-") {
                        lastNode = "Level3";
                        parentNode = data[i].level2 !== "-" ? data[i].level2 : null;
                        deptId = data[i].dept_Posn;
                    } else if (data[i].level2 && data[i].level2 !== "-") {
                        lastNode = "Level2";
                        parentNode = data[i].level1 !== "-" ? data[i].level1 : null;
                        deptId = data[i].dept_Posn;
                    }
                    $("#EPDept").val(deptId);
                    $("#ReportToDept").val(`${previousval}, ${parentNode}`);
                }
                //console.log($(tablebody).html());
            }).catch((error) => {
                //console.log(error);
            });
        } else {
            var tablebody = $("#EmpDeptLinkGrid tbody");
            $(tablebody).html("");//empty tbody
            api.get("/department/GetDept_Employee").then((data) => {
                //console.log(data);
                data = data.filter(i => i.level2 != "-" && i.employee_Id === empId && i.active == "Y");
                if (data.length === 0) {
                    // 2. Insert the "No Records Found" row
                    // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
                    const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
                    $(tablebody).append(noRecordsRow);
                }
                for (i = 0; i < data.length; i++) {
                    var previousval = $("#ReportToDept").val();
                    data[i].showMenu = "block" ;
                    $(tablebody).append(AppUtil.ProcessTemplateDataNew("EmpDeptLinkGridRow", data[i], i));
                    let lastNode = null;
                    let parentNode = null;
                    let deptId = 0;

                    if (data[i].level5 && data[i].level5 !== "-") {
                        lastNode = "Level5";
                        parentNode = data[i].level4 !== "-" ? data[i].level4 : null;
                        deptId = data[i].dept_Posn;
                    } else if (data[i].level4 && data[i].level4 !== "-") {
                        lastNode = "Level4";
                        parentNode = data[i].level3 !== "-" ? data[i].level3 : null;
                        deptId = data[i].dept_Posn;
                    } else if (data[i].level3 && data[i].level3 !== "-") {
                        lastNode = "Level3";
                        parentNode = data[i].level2 !== "-" ? data[i].level2 : null;
                        deptId = data[i].dept_Posn;
                    } else if (data[i].level2 && data[i].level2 !== "-") {
                        lastNode = "Level2";
                        parentNode = data[i].level1 !== "-" ? data[i].level1 : null;
                        deptId = data[i].dept_Posn;
                    }
                    $("#EPDept").val(deptId);
                    $("#ReportToDept").val(`${previousval}, ${parentNode}`);
                }
                //console.log($(tablebody).html());
            }).catch((error) => {
                //console.log(error);
            });
        }
    });
    $(document).on("change", "#UIDectShow", function () {
        var ARPId = $("#EPEmpId").val();
        if ($(this).is(":checked")) {
            var tablebody = $("#EmpUiGrid tbody");
            $(tablebody).html("");//empty tbody
            //if (isNaN(ARPId) || ARPId == 0) {
            //    ARPId = $("#EPEmpId").val();
            //} 
            api.getbulk("/Employee/GetEmplRoleUiList?employeeId=" + parseInt(ARPId)).then((data) => {
                data = data.filter(i => i.active == "N");
                //console.log(data);
                if (data.length === 0) {
                    // 2. Insert the "No Records Found" row
                    // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
                    const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
                    $(tablebody).append(noRecordsRow);
                }
                for (i = 0; i < data.length; i++) {
                    data[i].showMenu = data[i].fromDept === "N" ? "block" : "none";
                    data[i].showMenu = "none";
                    $(tablebody).append(AppUtil.ProcessTemplateDataNew("EmpUiGridRow", data[i], i));
                }
                //console.log(tablebody);
            }).catch((error) => { });
        } else {
            var tablebody = $("#EmpUiGrid tbody");
            $(tablebody).html("");//empty tbody
            //if (isNaN(ARPId) || ARPId == 0) {
            //    ARPId = $("#EPEmpId").val();
            //} 
            api.getbulk("/Employee/GetEmplRoleUiList?employeeId=" + parseInt(ARPId)).then((data) => {
                //console.log(data);
                if (data.length === 0) {
                    // 2. Insert the "No Records Found" row
                    // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
                    const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
                    $(tablebody).append(noRecordsRow);
                }
                data = data.filter(i => i.active == "Y");
                for (i = 0; i < data.length; i++) {
                    data[i].showMenu = data[i].fromDept === "N" ? "block" : "none";
                    $(tablebody).append(AppUtil.ProcessTemplateDataNew("EmpUiGridRow", data[i], i));
                }
                //console.log(tablebody);
            }).catch((error) => { });
        }
    });
    $('#deptEmpList').on('show.bs.modal', function (event) {
        var tablebody = $("#EmpDeptLink tbody");
        $(tablebody).html("");//empty tbody
        api.get("/Employee/GetDepartmentsLevel").then((data) => {
            //console.log(data);
            if (data.length === 0) {
                // 2. Insert the "No Records Found" row
                // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
                const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
                $(tablebody).append(noRecordsRow);
            }
            for (i = 0; i < data.length; i++) {
                $(tablebody).append(AppUtil.ProcessTemplateDataNew("EmpDeptLinkRow", data[i], i));
            }
            //console.log($(tablebody).html());
        }).catch((error) => {
            //console.log(error);
        });
    });
});
function loadLevels() {
    var Slvl2 = $("#Slvl2");
    var Slvl3 = $("#Slvl3");
    var Slvl4 = $("#Slvl4");
    var Slvl5 = $("#Slvl5");
    Slvl2.html('');
    Slvl3.html('');
    Slvl4.html('');
    Slvl5.html('');
    var defaultOpt = "<option value='0'>--Select--</option>";
    Slvl2.append(defaultOpt);
    Slvl3.append(defaultOpt);
    Slvl4.append(defaultOpt);
    Slvl5.append(defaultOpt);
    api.get("/department/GetDepartmentsLevel").then((data) => {
        data = data.filter(i => i.level2 != "-");
        let level2 = [...new Set(data.map(d => d.level2).filter(x => x))];
        let level3 = [...new Set(data.map(d => d.level3).filter(x => x))];
        let level4 = [...new Set(data.map(d => d.level4).filter(x => x))];
        let level5 = [...new Set(data.map(d => d.level5).filter(x => x))];

        // append options
        level2.forEach(v => Slvl2.append(`<option value="${v}">${v}</option>`));
        level3.forEach(v => Slvl3.append(`<option value="${v}">${v}</option>`));
        level4.forEach(v => Slvl4.append(`<option value="${v}">${v}</option>`));
        level5.forEach(v => Slvl5.append(`<option value="${v}">${v}</option>`));
    }).catch((error) => {
        //console.log(error);
    });
}
function LoadDeptEmp(employee_ID) {
    var tablebody = $("#EmpDeptLinkGrid tbody");
    $(tablebody).html("");//empty tbody
    $("#ReportToDept").val('');
    api.get("/department/GetDept_Employee").then((data) => {
        //console.log(data);
        data = data.filter(i => i.level2 != "-" && i.employee_Id === employee_ID && i.active == "Y");
        if (data.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        for (i = 0; i < data.length; i++) {
                    var previousval = $("#ReportToDept").val();
                    $(tablebody).append(AppUtil.ProcessTemplateDataNew("EmpDeptLinkGridRow", data[i], i));
                    let lastNode = null;
                    let parentNode = null;
                    let deptId = 0;

                    if (data[i].level5 && data[i].level5 !== "-") {
                        lastNode = "Level5";
                        parentNode = data[i].level4 !== "-" ? data[i].level4 : null;
                        deptId = data[i].dept_Posn;
                    } else if (data[i].level4 && data[i].level4 !== "-") {
                        lastNode = "Level4";
                        parentNode = data[i].level3 !== "-" ? data[i].level3 : null;
                        deptId = data[i].dept_Posn;
                    } else if (data[i].level3 && data[i].level3 !== "-") {
                        lastNode = "Level3";
                        parentNode = data[i].level2 !== "-" ? data[i].level2 : null;
                        deptId = data[i].dept_Posn;
                    } else if (data[i].level2 && data[i].level2 !== "-") {
                        lastNode = "Level2";
                        parentNode = data[i].level1 !== "-" ? data[i].level1 : null;
                        deptId = data[i].dept_Posn;
                    }
                    $("#EPDept").val(deptId);
                    $("#ReportToDept").val(`${previousval}, ${parentNode}`);
        }
        //console.log($(tablebody).html());
    }).catch((error) => {
        //console.log(error);
    });
}
function DeleteDeptEmp(element) {
    //var relatedTarget = $(element.relatedTarget);
    var employeeid = $(element).data("deptempid");
    var empid = $(element).data("empid");
    let confirmval = confirm("Are your sure you want to delete this ?", "Yes", "No");
    if (confirmval) {
        api.get("/Department/DelDept_Employee?designationId=" + parseInt(employeeid)).then((data) => {
            LoadDeptEmp(empid);
        }).catch((error) => {

        });
    }
}
function LoadDepartments() {
    var tablebody = $("#DeptP1Grid tbody");
    $(tablebody).html("");//empty tbody
    api.get("/department/GetUnassignedDepartments").then((data) => {
        //console.log(data);
        if (data.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        data = data.filter(i => i.level2 != "-");
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateDataNew("DeptP1GridRow", data[i], i));
        }
        //console.log($(tablebody).html());
    }).catch((error) => {
        //console.log(error);
    });
}
function LoadEmployee() {
    var tablebody = $("#EmployeeGrid tbody");
    $(tablebody).html("");//empty tbody
    $("#preloaderblurred").show();
    api.getbulk("/Employee/GetAllEmployee").then((data) => {
        //console.log(data);
        if (data.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateDataNew("EmployeeGridRow", data[i], i));
        }
        //console.log(tablebody);
        $("#preloaderblurred").hide();
    }).catch((error) => {
        $("#preloaderblurred").hide();
    });
    loadSelectEmployee();
}
function LoadOrgChart() {
    var tablebody = $("#OrgChartGrid tbody");
    $(tablebody).html("");//empty tbody
    api.getbulk("/Employee/GetAllOrgChart").then((data) => {
        //console.log(data);
        if (data.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateDataNew("OrgChartGridRow", data[i], i));
        }
        //console.log(tablebody);
    }).catch((error) => { });
    loadSelectEmployee();
}
function LoadRoleUiById(roleid) {
    var tablebody = $("#RPGrid tbody");
    $(tablebody).html("");//empty tbody
    var ARPId;
    if (roleid == 0) {
        ARPId = $("#ARPId").val();
    } else {
        ARPId = roleid;
    }
    api.getbulk("/Employee/GetRoleUiList?roleId=" + parseInt(ARPId)).then((data) => {
        //console.log(data);
        if (data.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateDataNew("RPGridRow", data[i], i));
        }
        //console.log(tablebody);
    }).catch((error) => { });
    //loadSelectEmployee();
}
function LoadRoleUiAll() {
    var tablebody = $("#RoleGrid tbody");
    $(tablebody).html("");
    $("#preloaderblurred").show();

    api.getbulk("/Employee/GetAllRoleUiList").then((data) => {
        $("#preloaderblurred").hide();

        if (!data || data.length === 0) {
            $(tablebody).append(`<tr><td colspan="20" class="text-center text-muted"><strong>No Records Found</strong></td></tr>`);
            return;
        }

        // ✅ Group entries by RoleName
        const grouped = {};
        data.forEach(item => {
            if (!grouped[item.roleName]) grouped[item.roleName] = [];
            grouped[item.roleName].push(item);
        });

        // ✅ Render rows
        for (const [roleName, list] of Object.entries(grouped)) {
            const hasMultiple = list.length > 1;
            const first = list[0];

            first.expandBtn = hasMultiple
                ? `<button class="btn btn-sm btn-link text-primary expand-btn" data-role="${roleName}" title="Expand">></button>`
                : "";

            // main row using your template
            $(tablebody).append(AppUtil.ProcessTemplateDataNew("RoleGridRow", first));

            // Add hidden sub-rows if multiple UI accesses
            if (hasMultiple) {
                for (let j = 1; j < list.length; j++) {
                    const sub = list[j];
                    const subRow = `
                        <tr class="sub-row d-none" data-parent="${roleName}">
                            <td>${sub.roleName}</td>
                            <td>${sub.uiLevel}</td>
                            <td>${sub.view_Allowed}</td>
                            <td>${sub.add_Edit_Allowed}</td>
                            <td>${sub.delete_Allowed}</td>
                            <td>${sub.approval_Allowed}</td>
                            <td></td>
                        </tr>`;
                    $(tablebody).append(subRow);
                }
            }
        }

        // ✅ Expand/Collapse toggle
        $("#RoleGrid").off("click", ".expand-btn").on("click", ".expand-btn", function () {
            const role = $(this).data("role");
            const subRows = $(`tr[data-parent='${role}']`);
            const isOpen = $(this).text() === "v";

            if (isOpen) {
                subRows.addClass("d-none");
                $(this).text(">");
            } else {
                subRows.removeClass("d-none");
                $(this).text("v");
            }
        });

    }).catch((err) => {
        $("#preloaderblurred").hide();
        console.error(err);
    });
}

function loadUiList() {
    var tablebody = $("#UiGrid tbody");
    $(tablebody).html("");//empty tbody
    var selElem = $('#UPPartOf');
    selElem.html('');
    div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
    selElem.append(div_data);
    api.getbulk("/Employee/GetAllUilist").then((data) => {
        //console.log(data);
        if (data.length === 0) {
            // 2. Insert the "No Records Found" row
            // We assume a standard table has a 6-column span (adjust 'colspan' as needed for your table)
            const noRecordsRow = `
                <tr>
                    <td colspan="20" style="text-align: center; color: #888;">
                        <strong>No Records Found</strong>
                    </td>
                </tr>`;
            $(tablebody).append(noRecordsRow);
        }
        for (i = 0; i < data.length; i++) {
            $(tablebody).append(AppUtil.ProcessTemplateDataNew("UiGridRow", data[i], i));
            div_data = "<option value='" + data[i].uiListId + "'>" + data[i].uI_Name_Label + "</option>";
            selElem.append(div_data);
        }
        //console.log(tablebody);
    }).catch((error) => { });
}
function loadDesignation() {
    var selElem = $('#EPDes');
    selElem.html('');
    api.getbulk("/designation/designations").then((data) => {

        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        selElem.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].designationId + "'>" + data[i].name + "</option>";
            selElem.append(div_data);
        }
    });
}
function loadDepartment(locationid) {
    var selElem = $('#EPDept');
    var selElemOrgDept = $('#OrgDept');
    selElem.html('');
    selElemOrgDept.html('');
    api.getbulk("/Department/GetDepartments").then((data) => {
        Departments = data;
        const filteredData = data.filter(item => item.plantId === locationid);
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        selElem.append(div_data);
        selElemOrgDept.append(div_data);
        for (i = 0; i < filteredData.length; i++) {
            div_data = "<option value='" + filteredData[i].departmentId + "'>" + filteredData[i].name + "</option>";
            selElemOrgDept.append(div_data);
            selElem.append(div_data);
        }
    });
}
function loadSelectEmployee() {
    //var OrgEmpl = $('#EPRoleReportTo');
    var OrgRoleReport = $('#OrgRoleReport');
    //OrgEmpl.html('');
    OrgRoleReport.html('');

    api.get("/Employee/GetAllEmployee").then((data) => {
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        //OrgEmpl.append(div_data);
        OrgRoleReport.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].employee_ID + "'>" + data[i].employee_name + "</option>";
            //OrgEmpl.append(div_data);
            OrgRoleReport.append(div_data);
        }
    }).catch((error) => {
    });
}
function loadSelectRole() {
    var OrgEmpl = $('#OrgRole');
    OrgEmpl.html('');

    api.get("/Employee/GetAllRoleList").then((data) => {
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        OrgEmpl.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].role_ListId + "'>" + data[i].role_Desc + "</option>";
            OrgEmpl.append(div_data);
        }
    }).catch((error) => {
    });
}
function loadLoaction() {
    var selElem = $('#EPloc');
    var selElemOrgLoc = $('#OrgLoc');
    selElem.html('');
    selElemOrgLoc.html('');

    api.get("/plant/getplants").then((data) => {
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        selElem.append(div_data);
        selElemOrgLoc.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].plantId + "'>" + data[i].name + "</option>";
            selElem.append(div_data);
            selElemOrgLoc.append(div_data);
        }
    }).catch((error) => {
    });
}
function loadSelectMenus() {
    var UiAccessRMenu1 = $('#UiAccessRMenu1');
    UiAccessRMenu1.html('');
    var UiAccessRMenu2 = $('#UiAccessRMenu2');
    UiAccessRMenu2.html('');
    var UiAccessRMenu3 = $('#UiAccessRMenu3');
    UiAccessRMenu3.html('');
    var UiAccessRMenu4 = $('#UiAccessRMenu4');
    UiAccessRMenu4.html('');
    var UiAccessRMenu5 = $('#UiAccessRMenu5');
    UiAccessRMenu5.html('');
    var SearchUim1 = $('#SearchUim1');
    SearchUim1.html('');

    api.get("/Employee/GetAllUilist").then((data) => {
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);
        UiAccessRMenu2.append(div_data);
        UiAccessRMenu3.append(div_data);
        UiAccessRMenu4.append(div_data);
        UiAccessRMenu5.append(div_data);
        SearchUim1.append(div_data);
        for (i = 0; i < data.length; i++) {
            menusdata = data;
            if (data[i].menuLevelId == 1) {
                div_data = "<option value='" + data[i].uiListId + "'>" + data[i].menu1 + "</option>";
                UiAccessRMenu1.append(div_data);
                SearchUim1.append(div_data);
            }
            //else if (data[i].menuLevelId == 2) {
            //    div_data = "<option value='" + data[i].uiListId + "'>" + data[i].menu2 + "</option>";
            //    UiAccessRMenu2.append(div_data);
            //}else if (data[i].menuLevelId == 3) {
            //    div_data = "<option value='" + data[i].uiListId + "'>" + data[i].menu3 + "</option>";
            //    UiAccessRMenu3.append(div_data);
            //}else if (data[i].menuLevelId == 4) {
            //    div_data = "<option value='" + data[i].uiListId + "'>" + data[i].menu4 + "</option>";
            //    UiAccessRMenu4.append(div_data);
            //}else if (data[i].menuLevelId == 5) {
            //    div_data = "<option value='" + data[i].uiListId + "'>" + data[i].menu5 + "</option>";
            //    UiAccessRMenu5.append(div_data);
            //}
        }
    }).catch((error) => {
    });
}
function loadSelectPermission() {
    var UiAccessRPermission = $('#UiAccessRPermission');
    UiAccessRPermission.html('');

    api.get("/Employee/GetAllPermission").then((data) => {
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRPermission.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].permissionId + "'>" + data[i].permission + "</option>";
            UiAccessRPermission.append(div_data);
        }
    }).catch((error) => {
    });
}

function DeleteOrgChart(element) {
    //var relatedTarget = $(element.relatedTarget);
    var orgid = $(element).data("orgid");
    var dept = $(element).data("dept");
    let confirmval = confirm("Are your sure you want to delete this ?", "Yes", "No");
    if (confirmval) {
        api.getbulk("/Employee/GetAllEmployee").then((data) => {
            const filteredData = data.filter(item => item.home_Dept_Id === dept);
            if (filteredData.length == 0) {
                api.get("/Employee/DelOrgChart?designationId=" + parseInt(orgid)).then((data) => {
                    LoadOrgChart();
                }).catch((error) => {

                });
            } else {
                alert("Please Delete The Employe With This Department.");
            }
        }).catch((error) => {

        });
    }
}
function DeleteEmployee(element) {
    //var relatedTarget = $(element.relatedTarget);
    var employeeid = $(element).data("employeeid");
    let confirmval = confirm("Are your sure you want to delete this ?", "Yes", "No");
    if (confirmval) {
        api.get("/Employee/GetAllOrgChart").then((data) => {
            if (!data.some(item => item.employee_Id === parseInt(employeeid))) {
                api.get("/Employee/DelEmployee?designationId=" + parseInt(employeeid)).then((data) => {
                    LoadEmployee();
                }).catch((error) => {

                });
            } else {
                alert("Please Delete Org Chart Of This Role.");
            }
        }).catch((error) => {

        });
    }
}
function ResetPassword(element) {
    //var relatedTarget = $(element.relatedTarget);
    var employeeid = $(element).data("employeeid");
    let confirmval = confirm("Are your sure you want to Reset Password Of the Employee ?", "Yes", "No");
    if (confirmval) {
        api.get("/Employee/ResetEmpPassword?EmpId=" + parseInt(employeeid)).then((data) => {

            var userrowData = {
                email: data.email,
                newPassword: data.password,
                confirmPassword: data.password,
                token: "ersasdsada"
            };

            const ipAddress = window.location.hostname;
            alert("Password Reset Successfully!");

            // Register employee
            api.post(`http://${ipAddress}:9003/account/ResetPassword`, userrowData, {
                headers: {
                    "Content-Type": "application/json",
                    "Accept": "application/json",
                },
            }).then((response) => {
                console.log("ResetPassword Success:", response);
            });
            LoadEmployee();
        }).catch((error) => {

        });
    }
}
function DeleteUiList(element) {
    //var relatedTarget = $(element.relatedTarget);
    var uilistid = $(element).data("uilistid");
    let confirmval = confirm("Are your sure you want to delete this ?", "Yes", "No");
    if (confirmval) {
        api.get("/Employee/CheckUiList?designationId=" + parseInt(uilistid)).then((data) => {
            if (data == true) {
                api.get("/Employee/DelUiList?designationId=" + parseInt(uilistid)).then((data) => {
                    loadUiList();
                }).catch((error) => {

                });
            } else {
                alert("Please Delete Role Ui Access Of This.");
            }
        }).catch((error) => {

        });
    }
}
function DeleteRoleList(element) {
    //var relatedTarget = $(element.relatedTarget);
    var uilistid = $(element).data("uilistid");
    let confirmval = confirm("Are your sure you want to delete this ?", "Yes", "No");
    if (confirmval) {
        api.get("/Employee/GetAllOrgChart").then((data) => {
            if (!data.some(item => item.role_NameId === parseInt(uilistid))) {
                api.get("/Employee/DelRoleList?designationId=" + parseInt(uilistid)).then((data) => {
                    LoadRoleUiAll();
                }).catch((error) => {

                });
            } else {
                alert("Please Delete Org Chart Of This Role.");
            }
        }).catch((error) => {

        });

    }
}
function DeleteRoleUiList(element) {
    //var relatedTarget = $(element.relatedTarget);
    var uilistid = $(element).data("uilistid");
    let confirmval = confirm("Are your sure you want to delete this ?", "Yes", "No");
    if (confirmval) {
        api.get("/Employee/DelRoleUiList?designationId=" + parseInt(uilistid)).then((data) => {
            LoadRoleUiAll();
            LoadRoleUiById();
        }).catch((error) => {

        });
    }
}

function DeleteEmplUiList(element) {
    //var relatedTarget = $(element.relatedTarget);
    var uilistid = $(element).data("uilistid");
    let confirmval = confirm("Are your sure you want to delete this ?", "Yes", "No");
    if (confirmval) {
        api.get("/Department/DelEmployee_UI_List?designationId=" + parseInt(uilistid)).then((data) => {
            LoadEmplUiById();
        }).catch((error) => {

        });
    }
}
function LoadEmplUiById() {
    var tablebody = $("#EmpUiGrid tbody");
    $(tablebody).html("");//empty tbody
    var ARPId = $("#EPEmpId").val();
    //if (isNaN(ARPId) || ARPId == 0) {
    //    ARPId = $("#EPEmpId").val();
    //} 
    api.getbulk("/Employee/GetEmplRoleUiList?employeeId=" + parseInt(ARPId)).then((data) => {
        const tablebody = "#EmpUiGrid tbody";
        $(tablebody).empty();

        if (!data || data.length === 0) {
            $(tablebody).append(`
            <tr>
                <td colspan="20" class="text-center text-muted"><strong>No Records Found</strong></td>
            </tr>
        `);
            return;
        }

        // ✅ Filter only active items
        data = data.filter(i => i.active === "Y");

        // ✅ Group by RoleName (handle empty ones)
        const grouped = {};
        data.forEach(item => {
            const roleKey = item.roleName && item.roleName.trim() !== "" ? item.roleName.trim() : "No Role (Direct UI Access)";
            if (!grouped[roleKey]) grouped[roleKey] = [];
            grouped[roleKey].push(item);
        });

        // ✅ Render grouped rows
        for (const [roleName, list] of Object.entries(grouped)) {
            const hasMultiple = list.length > 1;
            const first = list[0];
            first.showMenu = first.fromDept === "N" ? "block" : "none";
            first.expandBtn = hasMultiple
                ? `<button class="btn btn-sm btn-link text-primary expand-btn" data-role="${roleName}" title="Expand">></button>`
                : "";

            // main row using your template (EmpUiGridRow)
            $(tablebody).append(AppUtil.ProcessTemplateDataNew("EmpUiGridRow", first));

            // ✅ Add hidden sub-rows for other UI records
            if (hasMultiple) {
                for (let j = 1; j < list.length; j++) {
                    const sub = list[j];
                    const subRow = `
                    <tr class="sub-row d-none" data-parent="${roleName}">
                        <td></td>
                        <td></td>
                        <td>${sub.uiLevel ?? ""}</td>
                        <td>${sub.view_Allowed ?? ""}</td>
                        <td>${sub.add_Edit_Allowed ?? ""}</td>
                        <td>${sub.delete_Allowed ?? ""}</td>
                        <td>${sub.approval_Allowed ?? ""}</td>
                        <td>${sub.deact_dateStr ?? ""}</td>
                        <td>${sub.fromDept ?? ""}</td>
                    </tr>`;
                    $(tablebody).append(subRow);
                }
            }
        }

        // ✅ Expand/Collapse toggle
        $("#EmpUiGrid").off("click", ".expand-btn").on("click", ".expand-btn", function () {
            const role = $(this).data("role");
            const subRows = $(`tr[data-parent='${role}']`);
            const isOpen = $(this).text() === "v";

            if (isOpen) {
                subRows.addClass("d-none");
                $(this).text(">");
            } else {
                subRows.removeClass("d-none");
                $(this).text("v");
            }
        });

    }).catch((error) => {
        console.error(error);
    });
    //loadSelectEmployee();
}
function loadSelectMenusForEmpl() {
    var UiAccessRMenu1 = $('#UiAccessEMenu1');
    UiAccessRMenu1.html('');
    var UiAccessRMenu2 = $('#UiAccessEMenu2');
    UiAccessRMenu2.html('');
    var UiAccessRMenu3 = $('#UiAccessEMenu3');
    UiAccessRMenu3.html('');
    var UiAccessRMenu4 = $('#UiAccessEMenu4');
    UiAccessRMenu4.html('');
    var UiAccessRMenu5 = $('#UiAccessEMenu5');
    UiAccessRMenu5.html('');

    api.get("/Employee/GetAllUilist").then((data) => {
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRMenu1.append(div_data);
        UiAccessRMenu2.append(div_data);
        UiAccessRMenu3.append(div_data);
        UiAccessRMenu4.append(div_data);
        UiAccessRMenu5.append(div_data);
        menusdataEmpl = data;
        for (i = 0; i < data.length; i++) {
            if (data[i].menuLevelId == 1) {
                div_data = "<option value='" + data[i].uiListId + "'>" + data[i].menu1 + "</option>";
                UiAccessRMenu1.append(div_data);
            }
            //else if (data[i].menuLevelId == 2) {
            //    div_data = "<option value='" + data[i].uiListId + "'>" + data[i].menu2 + "</option>";
            //    UiAccessRMenu2.append(div_data);
            //} else if (data[i].menuLevelId == 3) {
            //    div_data = "<option value='" + data[i].uiListId + "'>" + data[i].menu3 + "</option>";
            //    UiAccessRMenu3.append(div_data);
            //} else if (data[i].menuLevelId == 4) {
            //    div_data = "<option value='" + data[i].uiListId + "'>" + data[i].menu4 + "</option>";
            //    UiAccessRMenu4.append(div_data);
            //} else if (data[i].menuLevelId == 5) {
            //    div_data = "<option value='" + data[i].uiListId + "'>" + data[i].menu5 + "</option>";
            //    UiAccessRMenu5.append(div_data);
            //}
        }
    }).catch((error) => {
    });
}
function loadSelectEmplPermission() {
    var UiAccessRPermission = $('#UiAccessEPermission');
    UiAccessRPermission.html('');

    api.get("/Employee/GetAllPermission").then((data) => {
        div_data = "<option value='" + 0 + "'>" + "--Select--" + "</option>";
        UiAccessRPermission.append(div_data);
        for (i = 0; i < data.length; i++) {
            div_data = "<option value='" + data[i].permissionId + "'>" + data[i].permission + "</option>";
            UiAccessRPermission.append(div_data);
        }
    }).catch((error) => {
    });
}