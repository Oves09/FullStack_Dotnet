const fs = require('fs').promises;
const EventEmitter = require('events');
const path = require('path');

const filePath = path.join(__dirname, '../data/books.json');
const bookEvents = new EventEmitter();

// Read all books
async function getAllBooks() {
    const data = await fs.readFile(filePath, 'utf-8');
    return JSON.parse(data);
}

// Get book by ID
async function getBookById(id) {
    const books = await getAllBooks();
    return books.find(book => book.id === id);
}

// Add a new book
async function addBook(newBook) {
    const books = await getAllBooks();
    books.push(newBook);
    await fs.writeFile(filePath, JSON.stringify(books, null, 2));
    bookEvents.emit('bookAdded', newBook);
}

// Update book
async function updateBook(id, updatedBook) {
    let books = await getAllBooks();
    const index = books.findIndex(book => book.id === id);
    if (index === -1) return null;
    books[index] = { ...books[index], ...updatedBook };
    await fs.writeFile(filePath, JSON.stringify(books, null, 2));
    bookEvents.emit('bookUpdated', books[index]);
    return books[index];
}

// Delete book
async function deleteBook(id) {
    let books = await getAllBooks();
    const index = books.findIndex(book => book.id === id);
    if (index === -1) return false;
    const deleted = books.splice(index, 1);
    await fs.writeFile(filePath, JSON.stringify(books, null, 2));
    bookEvents.emit('bookDeleted', deleted[0]);
    return true;
}

bookEvents.on('bookAdded', (book) => console.log(`Book Added: ${book.title}`));
bookEvents.on('bookUpdated', (book) => console.log(`Book Updated: ${book.title}`));
bookEvents.on('bookDeleted', (book) => console.log(`Book Deleted: ${book.title}`));

module.exports = { getAllBooks, getBookById, addBook, updateBook, deleteBook };
