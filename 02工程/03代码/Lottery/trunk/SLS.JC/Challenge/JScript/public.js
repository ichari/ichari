
function showWinOpen(url, name, iHeight, iWidth) {
    var iTop = (window.screen.availHeight - 30 - iHeight) / 2;       //获得窗口的垂直位置;
    var iLeft = (window.screen.availWidth - 10 - iWidth) / 2;           //获得窗口的水平位置;
    window.showModalDialog(url, name, "dialogHeight= " + iHeight + "px;dialogWidth= " + iWidth + "px;dialogTop= " + iTop + "px;dialogLeft= " + iLeft + "px;help=no;scroll=no;resizable=no;status=no;");
}