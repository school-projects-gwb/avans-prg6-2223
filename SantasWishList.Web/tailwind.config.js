/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        './Views/*/*.cshtml',
        './Areas/Identity/Pages/Account/*.cshtml',
        './Areas/Identity/Pages/*.cshtml',
        './wwwroot/js/*.js'
    ],
    theme: {
        extend: {
            colors: {
                'message-error': 'rgb(185 28 28)',
                'message-regular': 'rgb(91 33 182)',
                'message-success': 'rgb(21 128 61)'
            },
        },
    },
    plugins: [],
}