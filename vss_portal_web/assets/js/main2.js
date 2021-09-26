/*==================== SCROLL SECTIONS ACTIVE LINK ====================*/
const sections = document.querySelectorAll('section[id]')

function scrollActive() {
    const scrollY = window.pageYOffset

    sections.forEach(current => {
        const sectionHeight = current.offsetHeight
        const sectionTop = current.offsetTop - 70;
        sectionId = current.getAttribute('id')

        if (scrollY > sectionTop && scrollY <= sectionTop + sectionHeight) {
            document.querySelector('.nav-menu ul li a[href*=' + sectionId + ']').classList.add('active_link')
        } else {
            document.querySelector('.nav-menu ul li a[href*=' + sectionId + ']').classList.remove('active_link')
        }
    })
}
window.addEventListener('scroll', scrollActive)