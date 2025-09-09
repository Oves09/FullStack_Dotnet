import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <footer class="bg-light mt-auto py-4 border-top">
      <div class="container">
        <div class="row">
          <div class="col-md-6 mb-4 mb-md-0">
            <h5 class="fw-bold text-primary">
              <i class="fas fa-users me-2"></i>ProfileBook
            </h5>
            <p class="text-muted mb-2">Connect, Share, and Discover with your community.</p>
            <p class="text-muted small">&copy; 2025 ProfileBook. All rights reserved.</p>
          </div>
          <div class="col-md-3 mb-4 mb-md-0">
            <h6 class="fw-bold mb-3">Support</h6>
            <ul class="list-unstyled">
              <li><a routerLink="/help-center" class="text-muted text-decoration-none">Help Center</a></li>
              <li><a routerLink="/privacy-policy" class="text-muted text-decoration-none">Privacy Policy</a></li>
              <li><a routerLink="/terms" class="text-muted text-decoration-none">Terms of Service</a></li>
              <li><a routerLink="/contact" class="text-muted text-decoration-none">Contact Us</a></li>
            </ul>
          </div>
        </div>
        <hr class="my-4">
        <div class="row align-items-center">
          <div class="col-md-6 text-center text-md-start mb-3 mb-md-0">
            <p class="text-muted small mb-0">Built with Angular & ASP.NET Core</p>
          </div>
          <div class="col-md-6 text-center text-md-end">
            <div class="social-links d-inline-flex gap-3 justify-content-center justify-content-md-end">
              <a href="https://facebook.com/" target="_blank" rel="noopener" aria-label="Facebook" class="text-muted">
                <i class="fab fa-facebook-f"></i>
              </a>
              <a href="https://twitter.com/" target="_blank" rel="noopener" aria-label="Twitter" class="text-muted">
                <i class="fab fa-twitter"></i>
              </a>
              <a href="https://instagram.com/" target="_blank" rel="noopener" aria-label="Instagram" class="text-muted">
                <i class="fab fa-instagram"></i>
              </a>
              <a href="https://linkedin.com/" target="_blank" rel="noopener" aria-label="LinkedIn" class="text-muted">
                <i class="fab fa-linkedin-in"></i>
              </a>
            </div>
          </div>
        </div>
      </div>
    </footer>
  `,
  styles: [`
    footer {
      background-color: var(--light-bg, #f8f9fa) !important;
      border-top: 1px solid var(--border-color, #e0e0e0);
    }
    .social-links a {
      font-size: 1.25rem;
      transition: color 0.2s ease;
      padding: 0.25rem;
    }
    .social-links a:hover {
      color: var(--primary-color, #0d6efd) !important;
    }
    ul li {
      margin-bottom: 0.5rem;
    }
    ul li a:hover {
      color: var(--primary-color, #0d6efd) !important;
    }
    @media (max-width: 768px) {
      .text-md-end {
        text-align: center !important;
        margin-top: 1rem;
      }
      .text-md-start {
        text-align: center !important;
      }
      .mb-md-0 {
        margin-bottom: 0 !important;
      }
      .social-links {
        justify-content: center !important;
      }
    }
  `]
})
export class FooterComponent { }




// import { Component } from '@angular/core';
// import { CommonModule } from '@angular/common';
// import { RouterModule } from '@angular/router';

// @Component({
//   selector: 'app-footer',
//   standalone: true,
//   imports: [CommonModule, RouterModule],
//   template: `
//     <footer class="bg-light mt-auto py-4">
//       <div class="container">
//         <div class="row">
//           <div class="col-md-6">
//             <h5 class="fw-bold text-primary">
//               <i class="fas fa-users me-2"></i>ProfileBook
//             </h5>
//             <p class="text-muted mb-2">Connect, Share, and Discover with your community.</p>
//             <p class="text-muted small">&copy; 2025 ProfileBook. All rights reserved.</p>
//           </div>
  
//           <div class="col-md-3">
//             <h6 class="fw-bold mb-3">Support</h6>
//             <ul class="list-unstyled">
//               <li><a href="#" class="text-muted text-decoration-none">Help Center</a></li>
//               <li><a href="#" class="text-muted text-decoration-none">Privacy Policy</a></li>
//               <li><a href="#" class="text-muted text-decoration-none">Terms of Service</a></li>
//               <li><a href="#" class="text-muted text-decoration-none">Contact Us</a></li>
//             </ul>
//           </div>
//         </div>
//         <hr class="my-4">
//         <div class="row align-items-center">
//           <div class="col-md-6">
//             <p class="text-muted small mb-0">Built with Angular & ASP.NET Core</p>
//           </div>
//           <div class="col-md-6 text-md-end">
//             <div class="social-links">
//               <a href="#" class="text-muted me-3"><i class="fab fa-facebook-f"></i></a>
//               <a href="#" class="text-muted me-3"><i class="fab fa-twitter"></i></a>
//               <a href="#" class="text-muted me-3"><i class="fab fa-instagram"></i></a>
//               <a href="#" class="text-muted"><i class="fab fa-linkedin-in"></i></a>
//             </div>
//           </div>
//         </div>
//       </div>
//     </footer>
//   `,
//   styles: [`
//     footer {
//       background-color: var(--light-bg) !important;
//       border-top: 1px solid var(--border-color);
//     }

//     .social-links a {
//       font-size: 1.2rem;
//       transition: color 0.2s ease;
//     }

//     .social-links a:hover {
//       color: var(--primary-color) !important;
//     }

//     ul li {
//       margin-bottom: 0.5rem;
//     }

//     ul li a:hover {
//       color: var(--primary-color) !important;
//     }

//     @media (max-width: 768px) {
//       .text-md-end {
//         text-align: center !important;
//         margin-top: 1rem;
//       }
//     }
//   `]
// })
// export class FooterComponent { }
