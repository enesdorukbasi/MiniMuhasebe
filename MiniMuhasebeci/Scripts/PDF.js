function generatePDF() {
    const element = document.getElementById("pdfConvert");
    html2pdf()
        .from(element)
        .save();
}