// Customer Create View JavaScript
document.addEventListener('DOMContentLoaded', function() {
    // Auto-uppercase VAT number input
    const vatInput = document.querySelector('input[name="VatNumber"]');
    if (vatInput) {
        vatInput.addEventListener('input', function(e) {
            e.target.value = e.target.value.toUpperCase();
        });
    }

    // Real-time validation feedback
    document.querySelectorAll('input, textarea').forEach(input => {
        input.addEventListener('blur', function() {
            // Clear previous validation state
            this.classList.remove('border-red-500', 'border-green-500');
            
            // Add validation state based on validity
            if (this.value.trim() && this.checkValidity()) {
                this.classList.add('border-green-500');
            } else if (this.value.trim() && !this.checkValidity()) {
                this.classList.add('border-red-500');
            }
        });
    });

    // Form submission loading state
    const createForm = document.querySelector('form[asp-action="Create"]');
    if (createForm) {
        createForm.addEventListener('submit', function() {
            const submitBtn = document.querySelector('button[type="submit"]:not([onclick])');
            if (submitBtn) {
                submitBtn.disabled = true;
                submitBtn.innerHTML = `
                    <svg class="animate-spin w-4 h-4 mr-2" fill="none" viewBox="0 0 24 24">
                        <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>
                        <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                    </svg>
                    Creating...
                `;
            }
        });
    }
});