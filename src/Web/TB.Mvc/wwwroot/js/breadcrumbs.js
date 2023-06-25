// Function to generate the breadcrumbs based on the current view
function generateBreadcrumbs(viewName) {
    const breadcrumbsContainer = document.getElementById('breadcrumbs');
    const viewTitle = getViewTitle(viewName);

    // Clear existing breadcrumbs
    breadcrumbsContainer.innerHTML = '';

    // Generate breadcrumb items
    const breadcrumbHTML = `<li><a href="#">AnalytIQ</a></li><li>${viewTitle}</li>`;
    breadcrumbsContainer.innerHTML = breadcrumbHTML;
    breadcrumbsContainer.style.display = 'flex';
}

// Function to get the formatted title based on the view name
function getViewTitle() {

    // Retrieve the current view from the URL or any other source
    const path = window.location.pathname;
    const pathSegments = path.split('/').filter(segment => segment.trim() !== '');

    // Extract the last segment as the current view
    let currentView = pathSegments[pathSegments.length - 1];
    let substring = "View";
    if (currentView.includes(substring)) {
        currentView.replace('View', '').replace(/([A-Z])/g, ' $1').trim();


    }

    return currentView;
}

// Example usage:
const currentView = getViewTitle(); // Replace with the actual view name provided by the MVC method
generateBreadcrumbs(currentView);