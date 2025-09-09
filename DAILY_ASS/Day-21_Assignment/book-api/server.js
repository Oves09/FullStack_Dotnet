const express = require('express');
const { getAllBooks, getBookById, addBook, updateBook, deleteBook } = require('./services/bookService');
const app = express();
const PORT = 3000;

app.use(express.json());

// Root Route
app.get('/', (req, res) => {
    res.json({ message: "Welcome to Book Management API" });
});

// Get all books
app.get('/books', async (req, res) => {
    const books = await getAllBooks();
    res.json(books);
});

// Get book by ID
app.get('/books/:id', async (req, res) => {
    const book = await getBookById(req.params.id);
    if (!book) return res.status(404).json({ error: "Book not found" });
    res.json(book);
});

// Add book
app.post('/books', async (req, res) => {
    const { title, author } = req.body;
    if (!title || !author) return res.status(400).json({ error: "Title and author required" });
    const newBook = { id: Date.now().toString(), title, author };
    await addBook(newBook);
    res.status(201).json(newBook);
});

// Update book
app.put('/books/:id', async (req, res) => {
    const updatedBook = await updateBook(req.params.id, req.body);
    if (!updatedBook) return res.status(404).json({ error: "Book not found" });
    res.json(updatedBook);
});

// Delete book
app.delete('/books/:id', async (req, res) => {
    const success = await deleteBook(req.params.id);
    if (!success) return res.status(404).json({ error: "Book not found" });
    res.json({ message: "Book deleted successfully" });
});

app.listen(PORT, () => {
    console.log(`Server running on http://localhost:${PORT}`);
});
