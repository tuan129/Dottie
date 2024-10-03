document.addEventListener("DOMContentLoaded", function () {
	const images = document.querySelectorAll('.banner-image');
	let currentIndex = 1;
	images[currentIndex].classList.add('active');
	setInterval(showNextImage, 2000);
	function showNextImage() {
		images[currentIndex].classList.remove('active');
		currentIndex = (currentIndex + 1) % images.length;
		images[currentIndex].classList.add('active');
	}
});

const modal = document.querySelector('.js-modal');
const cartBtn = document.querySelector('.js-cart');
const modalContainer = document.querySelector('.js-modal-container');
const modalClose = document.querySelector('.js-modal-close');

function showCart() {
	modal.classList.add("open");
}
cartBtn.addEventListener('click', showCart);

function hideCart() {
	modal.classList.remove("open");
}
modalClose.addEventListener('click', hideCart);

modal.addEventListener('click', hideCart);

modalContainer.addEventListener('click', function (event) {
	event.stopPropagation();
})


/*UserModal*/

const modalUser = document.querySelector('.js-modal-user');
const userBtn = document.querySelector('.js-user');
const modalUserContainer = document.querySelector('.js-modal-container-user');
const modalCloseUser = document.querySelector('.js-modal-close-user');

function showUser() {
	modalUser.classList.add("open");
}
userBtn.addEventListener('click', showUser);

function hideUserModal() {
	modalUser.classList.remove("open");
}
modalCloseUser.addEventListener('click', hideUserModal);

modalUser.addEventListener('click', hideUserModal);

modalUserContainer.addEventListener('click', function (event) {
	event.stopPropagation();
})
