// Define the Contact interface
interface Contact {
    id: number;
    name: string;
    email: string;
    phone: string;
}

// ContactManager class to handle contact operations
class ContactManager {
    private contacts: Contact[] = [];
    private nextId: number = 1;

    // Add a new contact
    addContact(contact: Contact): void {
        if (!contact.name || !contact.email || !contact.phone) {
            throw new Error("All contact fields (name, email, phone) are required");
        }

        // Validate email format
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(contact.email)) {
            throw new Error("Invalid email format");
        }

        // Validate phone format (simple validation for demonstration)
        const phoneRegex = /^\+?[\d\s-]{10,}$/;
        if (!phoneRegex.test(contact.phone)) {
            throw new Error("Invalid phone number format");
        }

        // Assign ID and add to contacts
        contact.id = this.nextId++;
        this.contacts.push(contact);
        console.log(`Contact ${contact.name} added successfully with ID: ${contact.id}`);
    }

    // View all contacts
    viewContacts(): Contact[] {
        if (this.contacts.length === 0) {
            console.log("No contacts found");
            return [];
        }
        return [...this.contacts];
    }

    // Modify an existing contact
    modifyContact(id: number, updatedContact: Partial<Contact>): void {
        const contact = this.contacts.find(c => c.id === id);
        if (!contact) {
            throw new Error(`Contact with ID ${id} not found`);
        }

        // Validate updated fields if provided
        if (updatedContact.email && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(updatedContact.email)) {
            throw new Error("Invalid email format");
        }
        if (updatedContact.phone && !/^\+?[\d\s-]{10,}$/.test(updatedContact.phone)) {
            throw new Error("Invalid phone number format");
        }

        // Update contact fields
        Object.assign(contact, updatedContact);
        console.log(`Contact with ID ${id} modified successfully`);
    }

    // Delete a contact
    deleteContact(id: number): void {
        const index = this.contacts.findIndex(c => c.id === id);
        if (index === -1) {
            throw new Error(`Contact with ID ${id} not found`);
        }
        const deletedContact = this.contacts.splice(index, 1)[0];
        console.log(`Contact ${deletedContact.name} with ID ${id} deleted successfully`);
    }
}

// Test script
function testContactManager() {
    const manager = new ContactManager();

    console.log("\n=== Testing Contact Manager ===");

    // Test 1: Adding contacts
    console.log("\nTest 1: Adding contacts");
    try {
        manager.addContact({
            id: 0, // Will be overwritten by ContactManager
            name: "John Doe",
            email: "john.doe@example.com",
            phone: "+1234567890"
        });
        manager.addContact({
            id: 0,
            name: "Jane Smith",
            email: "jane.smith@example.com",
            phone: "+0987654321"
        });
    } catch (error) {
        console.error("Error adding contacts:", error.message);
    }

    // Test 2: Viewing contacts
    console.log("\nTest 2: Viewing all contacts");
    try {
        const contacts = manager.viewContacts();
        console.log("Current contacts:", contacts);
    } catch (error) {
        console.error("Error viewing contacts:", error.message);
    }

    // Test 3: Modifying a contact
    console.log("\nTest 3: Modifying a contact");
    try {
        manager.modifyContact(1, {
            name: "John Updated",
            email: "john.updated@example.com"
        });
        console.log("After modification:", manager.viewContacts());
    } catch (error) {
        console.error("Error modifying contact:", error.message);
    }

    // Test 4: Modifying non-existent contact
    console.log("\nTest 4: Modifying non-existent contact");
    try {
        manager.modifyContact(999, { name: "Ghost" });
    } catch (error) {
        console.error("Expected error:", error.message);
    }

    // Test 5: Deleting a contact
    console.log("\nTest 5: Deleting a contact");
    try {
        manager.deleteContact(2);
        console.log("After deletion:", manager.viewContacts());
    } catch (error) {
        console.error("Error deleting contact:", error.message);
    }

    // Test 6: Deleting non-existent contact
    console.log("\nTest 6: Deleting non-existent contact");
    try {
        manager.deleteContact(999);
    } catch (error) {
        console.error("Expected error:", error.message);
    }

    // Test 7: Adding contact with invalid email
    console.log("\nTest 7: Adding contact with invalid email");
    try {
        manager.addContact({
            id: 0,
            name: "Invalid User",
            email: "invalid-email",
            phone: "+1234567890"
        });
    } catch (error) {
        console.error("Expected error:", error.message);
    }
}

// Run the test
testContactManager();