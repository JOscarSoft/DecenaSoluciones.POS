window.themeHelper = {
    setTheme: function (theme) {
        if (theme === 'system') {
            localStorage.removeItem('app-theme');
            this.applyTheme(this.getSystemTheme());
        } else {
            localStorage.setItem('app-theme', theme);
            this.applyTheme(theme);
        }
    },

    getTheme: function () {
        return localStorage.getItem('app-theme') || 'system';
    },

    getSystemTheme: function () {
        return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
    },

    applyTheme: function (theme) {
        if (theme === 'dark') {
            document.documentElement.setAttribute('data-theme', 'dark');
        } else {
            document.documentElement.removeAttribute('data-theme');
        }
    },

    init: function () {
        const savedTheme = localStorage.getItem('app-theme');
        if (savedTheme) {
            this.applyTheme(savedTheme);
        } else {
            this.applyTheme(this.getSystemTheme());
        }

        // Listen for system theme changes
        window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', e => {
            if (!localStorage.getItem('app-theme')) {
                this.applyTheme(e.matches ? 'dark' : 'light');
            }
        });
    }
};

// Initialize immediately
window.themeHelper.init();
