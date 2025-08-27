function printHtmlString(htmlString) {
    if (htmlString) {
        var frame1 = document.createElement('iframe');
        frame1.name = "frame1";
        frame1.style.position = "absolute";
        frame1.style.top = "-1000000px";
        document.body.appendChild(frame1);
        var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
        frameDoc.document.open();
        frameDoc.document.write(htmlString);
        frameDoc.document.close();
        //var printWindow = window.open('', '_blank');
        //printWindow.document.open();
        //printWindow.document.write(htmlString);
        //printWindow.document.close();
        delay(2000).then(() => {
            //alert("here");
            
            window.frames["frame1"].focus();
            window.frames["frame1"].print();
            alert("printed");
            document.body.removeChild(frame1);

            //printWindow.print();
            //printWindow.onafterprint = window.close;
        });
    }
}
function delay(time) {
    return new Promise(resolve => setTimeout(resolve, time));
}