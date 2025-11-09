// Delete Confirmation Modal JavaScript
function confirmDelete(customerId, customerName) {
    const customerNameEl = document.getElementById('customerName');
    const deleteForm = document.getElementById('deleteForm');
    
    if (customerNameEl) {
        customerNameEl.textContent = customerName;
    }
    
    if (deleteForm) {
        // Build the delete URL - this will need to be set per view
        const baseUrl = window.customerDeleteBaseUrl || '/Customer/Delete';
        deleteForm.action = baseUrl + '/' + customerId;
    }
    
    const modal = document.getElementById('deleteModal');
    if (modal) {
        modal.classList.remove('hidden');
    }
}

function closeDeleteModal() {
    const modal = document.getElementById('deleteModal');
    if (modal) {
        modal.classList.add('hidden');
    }
}

// Escape key to close modal
document.addEventListener('keydown', function(event) {
    if (event.key === 'Escape') {
        closeDeleteModal();
    }
});