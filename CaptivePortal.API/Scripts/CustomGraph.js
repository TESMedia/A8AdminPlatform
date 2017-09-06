
       function saveToLocalStorage() {
				
        //var skillsSelect = document.getElementById("SiteDdl");
        //var selectedText = skillsSelect.options[skillsSelect.selectedIndex].text;
        //window.localStorage.setItem("SiteId", document.getElementById("SiteDdl").value);
        //window.localStorage.setItem('SiteName', selectedText);
        //var siteId = localStorage.getItem("SiteId");
        var SearchId = document.getElementById("ddlSelect").value;
	 
		
        //var dataObj = { SitedId: siteId, SiteName: selectedText, searchCategory: SearchId }
        var dataObj = {searchCategory: SearchId }
        alert("Inside CustomGraph");
        var arr = new Array();
        var arrData = new Array();
        var dataPieResult = new Array();
        var arrNetworkData = new Array();
        
        $.post("/RealTimeDataApi/GetAnalysisData", dataObj, function (resultJSON)
        {
           
            //for (i = 0; i <= 23; i++)
            
            //{
               
            //    arr.push(resultJSON[i].Name);
            //    arrData.push(resultJSON[i].Value);
		    // }
                
            for (key in resultJSON.ResultAreaGraph)
            {
                var result = resultJSON.ResultAreaGraph[key];
                arr.push(result.Name);
                arrData.push(result.Value);
            }


                document.getElementById("totalUsers").innerHTML = resultJSON.Result[0].TotalUsers;
                document.getElementById("totalSession").innerHTML = resultJSON.Result[0].TotalSessions;
                document.getElementById("totalRegisters").innerHTML = resultJSON.Result[0].RegisteredUsers;
		        document.getElementById("totalMaleUsers").innerHTML = resultJSON.Result[0].MaleUsers;
		        document.getElementById("totalFemaleUsers").innerHTML = resultJSON.Result[0].FemaleUsers;
		        document.getElementById("rowIOS").innerHTML = resultJSON.Result[0].NoOfIosUsers;
		        document.getElementById("rowAndroid").innerHTML = resultJSON.Result[0].NoOfAndriodUsers;
		        document.getElementById("rowWindow").innerHTML = resultJSON.Result[0].NoOfWindowUsers;
		        document.getElementById("rowOthers").innerHTML = resultJSON.Result[0].NoOfOthersUsers;
		       // //document.getElementById("percentageMale").innerHTML = resultJSON.Result[0].PercentageMale;
		       // document.getElementById("percentageFemale").innerHTML = resultJSON.Result[0].PercentageFemale;
		        dataPieResult.push(resultJSON.Result[0].NoOfAndriodUsers);
		        dataPieResult.push(resultJSON.Result[0].NoOfIosUsers);
		        dataPieResult.push(resultJSON.Result[0].NoOfWindowUsers);
		        dataPieResult.push(resultJSON.Result[0].NoOfOthersUsers);
		        arrNetworkData.push(resultJSON.Result[0].NetworkUsageUp);
		        arrNetworkData.push(resultJSON.Result[0].NetWorkUsageDown);


		        var ctxDoughNut = document.getElementById('PieChart').getContext('2d');
                //ctxRes.defaults.global.legend.display = false;
		        var myChart = new Chart(ctxDoughNut, {
		            type: 'doughnut',
		            data: {
		                labels: [],
		                datasets: [{
		                    backgroundColor: [
                              "#2ecc71",
                              "#3498db",
                              "#95a5a6",
                              "#9b59b6"
		                    ],
		                    data: dataPieResult
		                }]
		            }
		        });

		        var ctxNetwork = document.getElementById('NetworkPieChart').getContext('2d');
		        var myChart = new Chart(ctxNetwork, {
		            type: 'pie',
		            data: {
		                labels: ['NetworkUp','NetWorkDown'],
		                datasets: [{

		                    backgroundColor: [
                              "#2ecc71",
                              "#3498db",
		                    ],
		                    data: arrNetworkData
		                }]
		            }
		        });
            alert(arrData);
	            var ctx = document.getElementById('AreaChart').getContext('2d');
                var myChart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: arr,
                        fillColor: "rgba(252,147,65,0.5)",
                        strokeColor: "rgba(255,255,255,1)",
                        pointColor: "rgba(173,173,173,1)",
                        pointStrokeColor: "#fff",
                        datasets: [{
                            label: 'No Of average session',
                            data: arrData,
                            backgroundColor: "#2ecc71"

                        }]
                    }
                });

             

		    });
	}





