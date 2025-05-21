// Exporta um objeto com todas as funções necessárias
window.infiniteScroll = {
    // Inicializa o IntersectionObserver
    init: function (element, dotNetHelper) {
        console.log('Inicializando IntersectionObserver...');

        const options = {
            root: null,
            rootMargin: '250px',
            threshold: 0.01
        };

        const observer = new IntersectionObserver(async (entries) => {
            const entry = entries[0];
            console.log('Observer triggered:', entry.isIntersecting);

            if (entry.isIntersecting) {
                console.log('Elemento sentinela visível - carregando mais itens');
                try {
                    await dotNetHelper.invokeMethodAsync('LoadMoreItems');
                } catch (error) {
                    console.error('Erro ao chamar LoadMoreItems:', error);
                }
            }
        }, options);

        observer.observe(element);
        console.log('Observer configurado com sucesso');

        return {
            dispose: () => {
                console.log('Dispose do observer chamado');
                observer.disconnect();
            }
        };
    },

    // Verifica se o elemento está visível
    checkVisibility: function (element) {
        const rect = element.getBoundingClientRect();
        const isVisible = (
            rect.top <= (window.innerHeight || document.documentElement.clientHeight) &&
            rect.bottom >= 0 &&
            rect.left <= (window.innerWidth || document.documentElement.clientWidth) &&
            rect.right >= 0
        );

        console.log('Verificando visibilidade do sentinela:', isVisible);
        return isVisible;
    }
};