document.getElementById('ping').addEventListener('click', async () => {
  const res = await fetch('/hello');
  const data = await res.json();
  document.getElementById('out').textContent = JSON.stringify(data, null, 2);
});
