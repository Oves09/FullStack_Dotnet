```markdown
# Getting Started with Angular

## Installing Angular CLI

### Step 1: Install Node.js
Download and install Node.js from https://nodejs.org/

### Step 2: Install Angular CLI
```bash
npm install -g @angular/cli
```

### Step 3: Verify Installation
```bash
ng version
```

## JIT vs AOT Compilation

### Just-In-Time (JIT) Compilation
- Templates and components are compiled in the browser
- Used during development
- Slower initial load time
- Larger bundle size

### Ahead-of-Time (AOT) Compilation
- Templates are pre-compiled during build process
- Default for production builds
- Faster rendering
- Early detection of template errors
- Better tree-shaking
- Smaller bundle size
```