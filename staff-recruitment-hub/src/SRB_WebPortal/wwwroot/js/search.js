const keywordInput = document.getElementById('jobSearch');
const locationSelect = document.getElementById('locationFilter');
const jobList = document.getElementById('jobList');

function searchJobs() {
   const keyword = keywordInput.value;
   const location = locationSelect.value;

   fetch(`/Home/Search?keyword=${keyword}&locationId=${location}`)
      .then((res) => res.json())
      .then((data) => {
         jobList.innerHTML = '';

         data.forEach((job) => {
            const html = `
                <div class="bg-white p-6 rounded-xl shadow hover:shadow-lg transition">

                    <h3 class="font-semibold text-lg mb-2">
                        ${job.title}
                    </h3>

                    <p class="text-gray-500 mb-2">
                        ${job.companyName}
                    </p>

                    <p class="text-emerald-600 font-semibold mb-3">
                        ${job.salary}
                    </p>

                    <div class="flex gap-6 text-sm text-gray-400">

                        <span class="bg-emerald-100 px-2 py-1 text-xs rounded-full">
                            ${job.location.locationName}
                        </span>

                        <span class="bg-emerald-100 px-2 py-1 text-xs rounded-full">
                            ${job.jobType}
                        </span>

                    </div>

                </div>
                `;

            jobList.innerHTML += html;
         });
      });
}

keywordInput.addEventListener('keyup', searchJobs);
locationSelect.addEventListener('change', searchJobs);
