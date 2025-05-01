// wwwroot/js/infiniteScroll.js
window.infiniteScroll = {
    init: function (element, dotNetHelper) {
        const observer = new IntersectionObserver(async (entries) => {
            if (entries[0].isIntersecting) {
                try {
                    await dotNetHelper.invokeMethodAsync('LoadMoreItems');
                } catch (error) {
                    console.error('Error calling LoadMoreItems:', error);
                }
            }
        }, {
            root: null,
            rootMargin: '200px',
            threshold: 0.1
        });

        observer.observe(element);

        return {
            dispose: () => {
                console.log('Disposing observer');
                observer.disconnect();
            }
        };
    }
};

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