using System;
using System.Threading.Tasks;
using Microsoft.Azure.Batch;
using Microsoft.Azure.Batch.Auth;
using Microsoft.Azure.Batch.Common;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var credentials = new BatchSharedKeyCredentials("get these credentials",
                "from the portal.azure",
                "for the batch service");

            using (var batchClient = BatchClient.Open(credentials))
            {
                var job = batchClient.JobOperations.CreateJob();
                job.PoolInformation = new PoolInformation()
                {
                    AutoPoolSpecification = new AutoPoolSpecification()
                    {
                        AutoPoolIdPrefix = "HelloWorld",
                        PoolSpecification = new PoolSpecification()
                        {
                            TargetDedicatedComputeNodes = 2,
                            CloudServiceConfiguration = new CloudServiceConfiguration("5"),
                            VirtualMachineSize = "standard_d1_v2"
                        },
                        KeepAlive = false,
                        PoolLifetimeOption = PoolLifetimeOption.Job
                    }
                };
                await job.CommitAsync();
                batchClient.JobOperations.AddTask(job.Id, new CloudTask("fdgd", "echo Hello"));
                
            }
        }
    }
}
