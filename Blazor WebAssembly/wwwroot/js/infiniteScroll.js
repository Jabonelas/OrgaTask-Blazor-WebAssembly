// wwwroot/js/infiniteScroll.js
window.infiniteScroll = {
    init: function (sentinel, dotNetHelper) {
        console.log('[Scroll] Iniciando observer...');

        // Variável para controlar o estado de carregamento
        let isLoading = false;

        const observer = new IntersectionObserver(async (entries) => {
            // Verifica se o sentinela está visível E não está em processo de carregamento
            if (entries[0].isIntersecting && !isLoading) {
                console.log('[Scroll] Sentinela visível - carregando mais itens');
                isLoading = true;

                try {
                    await dotNetHelper.invokeMethodAsync('LoadMoreItems');
                } catch (error) {
                    console.error('[Scroll] Erro ao carregar itens:', error);
                } finally {
                    // Adiciona um pequeno delay antes de permitir nova carga
                    setTimeout(() => isLoading = false, 500);
                }
            }
        }, {
            root: null,
            rootMargin: '500px', // Dispara 500px antes de chegar no final
            threshold: 0.01
        });

        observer.observe(sentinel);
        console.log('[Scroll] Observer configurado com sucesso');

        // Para debug (pode acessar via console do navegador)
        window.scrollObserver = observer;
    }
};

//window.infiniteScroll = {
//    init: function (element, dotNetHelper) {
//        const observer = new IntersectionObserver(async (entries) => {
//            if (entries[0].isIntersecting) {
//                try {
//                    await dotNetHelper.invokeMethodAsync('LoadMoreItems');
//                } catch (error) {
//                    console.error('Error calling LoadMoreItems:', error);
//                }
//            }
//        }, {
//            root: null,
//            rootMargin: '200px',
//            threshold: 0.1
//        });

//        observer.observe(element);

//        return {
//            dispose: () => {
//                console.log('Disposing observer');
//                observer.disconnect();
//            }
//        };
//    }
//};

//window.tarefasInfiniteScroll = {
//    init: function (element, dotNetHelper) {
//        const observer = new IntersectionObserver(async (entries) => {
//            if (entries[0].isIntersecting) {
//                await dotNetHelper.invokeMethodAsync('LoadMoreItems');
//            }
//        }, {
//            root: null,
//            rootMargin: '200px',
//            threshold: 0.1
//        });

//        observer.observe(element);

//        return {
//            dispose: () => observer.disconnect()
//        };
//    }
//};