// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function openPdfModal(pdfUrl, title) {
    if (!pdfUrl) return;

    document.getElementById("pdfViewerFrame").src = pdfUrl;
    document.querySelector("#pdfViewerModal .modal-title")
        .textContent = title || "PDF Önizleme";

    new bootstrap.Modal(
        document.getElementById("pdfViewerModal")
    ).show();
}


