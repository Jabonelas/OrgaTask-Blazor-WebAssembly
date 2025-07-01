export function initObserver(sentinel, dotNetHelper) {
    const observer = new IntersectionObserver(
        (entries) => {
            if (entries[0].isIntersecting) {
                dotNetHelper.invokeMethodAsync('CarregandoMaisItensAsync');
            }
        },
        { threshold: 0.1 }
    );

    observer.observe(sentinel);
    return observer;
}

export function checkVisibility(sentinel) {
    const rect = sentinel.getBoundingClientRect();
    return rect.top <= window.innerHeight && rect.bottom >= 0;
}

export function dispose(observer) {
    observer.disconnect();
}