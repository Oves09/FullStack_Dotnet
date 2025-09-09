
use onlineRetail

db.products.insertOne({
  name: "iPhone 15",
  category: "Electronics",
  price: 79999,
  inStock: true,
  brand: "Apple",
  specs: {
    storage: "128GB",
    color: "Black"
  }
})

db.products.insertOne({
  name: "Nike Running Shoes",
  category: "Footwear",
  price: 5999,
  inStock: true,
  brand: "Nike",
  sizes: [7, 8, 9, 10]
})

db.users.insertOne({
  username: "john_doe",
  email: "john@example.com",
  hashedPassword: "hashed1234password"
})

db.users.insertOne({
  name: "Rushikesh",
  email: "rushikesh@example.com"
})

db.products.insertMany([
  { name: "iPhone 15 Pro Max", price: 79999 },
  { name: "AirPods Pro", price: 5999 }
])

db.orders.insertOne({
  userId: ObjectId("6890942fe805637f27eec4ac"),
  orderDate: new Date(),
  products: [
    {
      productId: ObjectId("689093e6e805637f27eec4a9"),
      quantity: 1,
      price: 79999
    }
  ],
  totalAmount: 79999,
  paymentStatus: "Paid"
})

db.products.createIndex({ category: 1 })

db.products.find({ category: "Electronics" })

db.users.find()

db.orders.find({ userId: ObjectId("6890942fe805637f27eec4ac") })
