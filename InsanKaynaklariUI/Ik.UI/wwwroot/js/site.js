// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



//************Tempdata bilgilendirmeleri icin************
function showNotification(type, message) {
    var notificationArea = document.getElementById('notification-area');
    var alertDiv = document.createElement('div');
    alertDiv.classList.add('alert');
    if (type === 'success') {
        alertDiv.classList.add('alert-success');
    } else if (type === 'error') {
        alertDiv.classList.add('alert-danger');
    } else if (type === 'warning') {
        alertDiv.classList.add('alert-warning');
    }
    alertDiv.textContent = message;
    notificationArea.appendChild(alertDiv);

    // Fade-in animasyonu
    alertDiv.style.opacity = 0;
    var opacity = 0;
    var fadeInInterval = setInterval(function () {
        opacity += 0.1;
        alertDiv.style.opacity = opacity;
        if (opacity >= 1) {
            clearInterval(fadeInInterval);

            // Belirli bir süre sonra fade-out animasyonunu başlat
            setTimeout(function () {
                // Fade-out animasyonu
                var fadeOutInterval = setInterval(function () {
                    opacity -= 0.1;
                    alertDiv.style.opacity = opacity;
                    if (opacity <= 0) {
                        clearInterval(fadeOutInterval);
                        alertDiv.style.display = 'none';
                        alertDiv.remove();
                    }
                }, 100);
            }, 3000); // 3000 milisaniye (3 saniye) sonra fade-out animasyonu başlasın
        }
    }, 50); // 100 milisaniyede bir fade-in animasyonunu güncelle
}
//************Tempdata bilgilendirmeleri icin************



function deleteAdvance(advanceId) {
    var confirmation = confirm('Are you sure you want to delete this item?');
    if (confirmation) {
        fetch('/EmployeeArea/Advance/Delete/' + advanceId, {
            method: 'DELETE'
        })
            .then(response => {
                if (response.ok) {
                    location.reload();
                } else {
                    location.reload();
                }
            });
    }
}

function deleteExpense(expenseId) {
    var confirmation = confirm('Harcama isteğini silmek istediğinize emin misiniz?');
    if (confirmation) {
        fetch('/EmployeeArea/ExpenseRequests/Delete/' + expenseId, {
            method: 'DELETE'
        })
            .then(response => {
                if (response.ok) {
                    location.reload();
                } else {
                    location.reload();
                }
            });
    }
}

function deleteLeave(leaveId) {
    var confirmation = confirm('Are you sure you want to delete this item?');
    if (confirmation) {
        fetch('/EmployeeArea/LeaveRequest/Delete/' + leaveId, {
            method: 'DELETE'
        })
            .then(response => {
                if (response.ok) {
                    location.reload();
                } else {
                    location.reload();
                }
            });
    }
}

function Delete(companyId) {
    var confirmation = confirm('Are you sure you want to delete this item?');
    if (confirmation) {
        fetch('/Admin/Company/Delete/' + companyId, {
            method: 'DELETE'
        })
            .then(response => {
                if (response.ok) {
                    location.reload();
                } else {
                    location.reload();
                }
            });
    }
}
