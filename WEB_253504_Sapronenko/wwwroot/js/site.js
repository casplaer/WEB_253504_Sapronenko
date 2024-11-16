document.addEventListener("DOMContentLoaded", function() {
    document.addEventListener("click", function(event) {
        if (event.target.classList.contains("page-link")) {
            event.preventDefault();

            const url = event.target.getAttribute("href");

            fetch(url, {
                headers: {
                    "X-Requested-With": "XMLHttpRequest" 
                }
            })
            .then(response => response.text())
            .then(data => {
                document.querySelector("#heroListContainer").innerHTML = data;
            })
            .catch(error => console.error("Ошибка загрузки данных:", error));
        }
    });
});
