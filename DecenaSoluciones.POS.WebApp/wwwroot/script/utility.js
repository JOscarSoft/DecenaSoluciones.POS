function printHtmlString(htmlString) {
    if (htmlString) {
        var printWindow = window.open('', '_blank');
        printWindow.document.open();
        printWindow.document.write(htmlString);
        printWindow.document.close();
        delay(500).then(() => {
            printWindow.print();
            printWindow.onafterprint = window.close;   
        });
    }
}
function delay(time) {
    return new Promise(resolve => setTimeout(resolve, time));
}

window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
}