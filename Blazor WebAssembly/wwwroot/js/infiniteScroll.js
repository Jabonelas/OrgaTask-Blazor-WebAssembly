window.infiniteScroll = {
    init: function (element, dotnetHelper) {
        const observer = new IntersectionObserver(async (entries) => {
            if (entries[0].isIntersecting) {
                try {
                    await dotnetHelper.invokeMethodAsync('LoadMoreItems');
                } catch (error) {
                    console.error('Error calling LoadMoreItems:', error);
                }
            }
        }, {
            rootMargin: '200px' // Carrega antes de chegar no final
        });

        observer.observe(element);
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