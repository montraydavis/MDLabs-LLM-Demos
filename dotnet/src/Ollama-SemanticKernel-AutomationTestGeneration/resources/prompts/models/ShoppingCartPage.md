Create a ShoppingCartPage class for an e-commerce web application.
Page Name: ShoppingCartPage
[Elements]
- Cart Items List (selector: '#cart-items')
- Cart Item (selector: '.cart-item', multiple elements)
    - Product Name (selector: '.product-name')
    - Quantity Input (selector: '.quantity-input')
    - Remove Button (selector: '.remove-item')
- Checkout Button (selector: '#checkout-button')
[/Elements]
[Methods]
- UpdateItemQuantity method that takes a product name and new quantity as parameters and updates the quantity for the specified item.
- RemoveItem method that takes a product name as a parameter and removes the item from the cart.
- ProceedToCheckout method that clicks the checkout button.
[/Methods]
----------------------------------------
Ensure the entire structure is extracted recursively.
Ensure you respond only with the generated C# source code -- no additional text, comments, or formatting.