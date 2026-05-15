const API = "http://localhost:5005/api/employees";
let curId = null;
let charts = {};


function toggleTheme() {
    const h = document.documentElement;
    const isDark = h.getAttribute('data-theme') === 'dark';
    h.setAttribute('data-theme', isDark ? 'light' : 'dark');

    
    Chart.defaults.color = isDark ? '#212529' : '#e9ecef';
    if (curId) load();
}


function tab(type) {
    document.getElementById('indArea').style.display = type === 'ind' ? 'block' : 'none';
    document.getElementById('compArea').style.display = type === 'comp' ? 'block' : 'none';

    document.getElementById('t1').className = type === 'ind' ? 'tab active' : 'tab';
    document.getElementById('t2').className = type === 'comp' ? 'tab active' : 'tab';
}


async function load() {
    try {
        const r = await fetch(API);
        const emps = await r.json();

        const L = document.getElementById("list");
        const s1 = document.getElementById("s1");
        const s2 = document.getElementById("s2");

        L.innerHTML = "<h3>İşçilər</h3>";
        s1.innerHTML = "";
        s2.innerHTML = "";

        emps.forEach(e => {
            
            const d = document.createElement("div");
            d.className = `emp-item ${e.id === curId ? 'active' : ''}`;
            d.innerText = `${e.firstName} ${e.lastName}`;
            d.onclick = () => {
                curId = e.id;
                load();
            };
            L.appendChild(d);

            
            const o = `<option value="${e.id}">${e.firstName}</option>`;
            s1.innerHTML += o;
            s2.innerHTML += o;
        });

        
        if (curId) {
            const e = emps.find(x => x.id === curId);
            document.getElementById("editZ").style.display = "block";
            document.getElementById("curN").innerText = `${e.firstName} ${e.lastName} - İllik Analiz`;

            
            const labels = e.records.map(r => r.monthName);
            const performanceData = e.records.map(r => r.finalScore);
            const attendanceData = e.records.map(r => r.attendance);

            draw('c1', 'line', labels, performanceData, 'Ümumi Performans %');
            draw('c2', 'radar', labels, attendanceData, 'Davamiyyət %');
        }
    } catch (err) {
        console.error("Məlumat yüklənərkən xəta:", err);
    }
}


function draw(id, type, labels, data, label) {
    if (charts[id]) charts[id].destroy(); 

    const ctx = document.getElementById(id).getContext('2d');
    charts[id] = new Chart(ctx, {
        type: type,
        data: {
            labels: labels,
            datasets: [{
                label: label,
                data: data,
                borderColor: '#0d6efd',
                backgroundColor: 'rgba(13,110,253,0.1)',
                fill: true,
                tension: 0.3
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: type === 'line' ? { y: { min: 0, max: 100 } } : { r: { min: 0, max: 100 } }
        }
    });
}


async function addEmp() {
    const fn = document.getElementById("fn").value;
    const ln = document.getElementById("ln").value;

    if (!fn) return alert("Ad daxil edin!");

    await fetch(API, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ firstName: fn, lastName: ln || "-" })
    });

    document.getElementById("fn").value = "";
    document.getElementById("ln").value = "";
    load();
}


async function upd() {
    if (!curId) return;

    const data = {
        monthIndex: parseInt(document.getElementById("mI").value),
        task: parseFloat(document.getElementById("t").value || 0),
        quality: parseFloat(document.getElementById("q").value || 0),
        attendance: parseFloat(document.getElementById("a").value || 0)
    };

    await fetch(`${API}/${curId}/update-single-month`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    });

    load(); 
}


async function doComp() {
    const id1 = document.getElementById("s1").value;
    const id2 = document.getElementById("s2").value;

    const r = await fetch(`${API}/compare?id1=${id1}&id2=${id2}`);
    const d = await r.json();

    draw('c3', 'line', d.first.labels, d.first.performanceHistory, d.first.firstName);
    draw('c4', 'radar', d.first.labels, d.first.attendanceHistory, d.first.firstName);
}


async function importEx() {
    const fileInput = document.getElementById("fileInp");
    if (fileInput.files.length === 0) return;

    const formData = new FormData();
    formData.append("file", fileInput.files[0]);

    await fetch(`${API}/import`, {
        method: "POST",
        body: formData
    });

    alert("Məlumatlar uğurla daxil edildi!");
    load();
}


load();
