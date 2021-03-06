let gui_controller = {
    simulatorGUI: document.querySelector("#simulatorGUI"),
    getLayerPanel: () => { return this.simulatorGUI.querySelector("#layerPanel") },
    getInputNodes: () => { return this.simulatorGUI.querySelector("#layerPanel").getElementsByTagName('INPUT'); },
    getInformaitonPanel: () => { return this.simulatorGUI.querySelector("#information"); },
    changeBoardName: (name) => { this.simulatorGUI.querySelector("#simulatorView span").innerHTML = name; },
    showGUI: () => { simulatorGUI.style.visibility = "visible"; }
}