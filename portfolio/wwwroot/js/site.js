document.addEventListener("DOMContentLoaded", () => {
    const targets = document.querySelectorAll(".magnetic-text, .magnetic-word");

    function splitToChars(element) {
        if (!element || element.dataset.split === "true") return;

        const text = element.textContent;
        const fragment = document.createDocumentFragment();

        for (const ch of text) {
            if (ch === " ") {
                fragment.appendChild(document.createTextNode(" "));
            } else {
                const span = document.createElement("span");
                span.className = "char";
                span.textContent = ch;
                fragment.appendChild(span);
            }
        }

        element.textContent = "";
        element.appendChild(fragment);
        element.dataset.split = "true";
    }

    targets.forEach(splitToChars);

    targets.forEach((el) => {
        el.addEventListener("mousemove", (e) => {
            const rect = el.getBoundingClientRect();
            const chars = el.querySelectorAll(".char");

            chars.forEach((char) => {
                const charRect = char.getBoundingClientRect();
                const charCenterX = charRect.left + charRect.width / 2;
                const charCenterY = charRect.top + charRect.height / 2;

                const dx = e.clientX - charCenterX;
                const dy = e.clientY - charCenterY;
                const distance = Math.sqrt(dx * dx + dy * dy);

                const isHighlight = el.classList.contains("magnetic-word");
                const radius = isHighlight ? 75 : 55;
                const strength = isHighlight ? 10 : 5;

                if (distance < radius) {
                    const pull = (1 - distance / radius);
                    const moveX = (-dx / radius) * strength * pull;
                    const moveY = (-dy / radius) * strength * pull;

                    char.style.transform = `translate(${moveX}px, ${moveY}px)`;
                } else {
                    char.style.transform = "translate(0, 0)";
                }
            });
        });

        el.addEventListener("mouseleave", () => {
            const chars = el.querySelectorAll(".char");
            chars.forEach((char) => {
                char.style.transform = "translate(0, 0)";
            });
        });
    });
});