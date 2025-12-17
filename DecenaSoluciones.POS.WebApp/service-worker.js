// In development, always fetch from the network and do not enable offline support.
// This is because caching would make development dangerous (do not want to render stale content)
self.addEventListener('fetch', () => { });
