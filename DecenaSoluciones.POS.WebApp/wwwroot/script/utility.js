function printHtmlString(htmlString) {
    if (htmlString) {
        var printWindow = window.open('', '_blank');
        printWindow.document.open();
        printWindow.document.write(htmlString);
        printWindow.document.close();
        delay(500).then(() => {
            printWindow.print();
            printWindow.close();
        });
    }
}
function delay(time) {
    return new Promise(resolve => setTimeout(resolve, time));
}