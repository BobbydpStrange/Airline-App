namespace AirlineManager.ClientTests;

using AirlineManager.Blazor.Pages.Maintenance.MaintenanceScheduling;
using AirlineManager.Blazor.Services;
using AirlineManager.Shared.Data;
using AngleSharp.Dom;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TestContext = Bunit.TestContext;

/// <summary>
/// These tests are written entirely in C#.
/// Learn more at https://bunit.dev/docs/getting-started/writing-tests.html#creating-basic-tests-in-cs-files
/// </summary>
public class MaintenanceScheduling : BunitTestContext
{
	[Test]
	public void ComponentRenders()
	{
		using var ctx = new TestContext();
		ctx.JSInterop.Mode = JSRuntimeMode.Loose;
		var mocked = new Mock<IMaintenanceDataService>();

		ctx.Services.AddScoped(e => mocked.Object);
		var cut = ctx.RenderComponent<MaintenanceSchedulingAssistance>();
		Assert.That(cut, Is.Not.Null);
		var heading = cut.Find("h3").GetInnerText();
		Assert.AreEqual(heading, "Maintenance Scheduling");
	}

	[Test]
	public void SelectingPlaneShowsHangerSelect()
	{
		using var ctx = new TestContext();
		ctx.JSInterop.Mode = JSRuntimeMode.Loose;
		var mocked = new Mock<IMaintenanceDataService>();

		//mocked.Setup(m => m.GetPlanesDueForMaintenance())
		//	.Returns( Task.FromResult(new List<PlanesDueForMaintenance> { new PlanesDueForMaintenance() { } }));

		ctx.Services.AddScoped(e => mocked.Object);
		var cut = ctx.RenderComponent<MaintenanceSchedulingAssistance>();

		var hangercard = cut.FindAll("h3").Where(h => h.GetInnerText() == "Select A Hanger").Count();
		Assert.That(hangercard == 0);

        cut.SetParametersAndRender(parameters => parameters
		.Add(p => p.SelectedPlane, new PlanesDueForMaintenance() { }));

        hangercard = cut.FindAll("h3").Where(h => h.GetInnerText() == "Select A Hanger").Count();
		Assert.That(hangercard > 0);
	}

    [Test]
    public void SelectingPlaneShowsWorkerSelect()
    {
        using var ctx = new TestContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        var mocked = new Mock<IMaintenanceDataService>();

        ctx.Services.AddScoped(e => mocked.Object);
        var cut = ctx.RenderComponent<MaintenanceSchedulingAssistance>();

        var workerselect = cut.FindAll("h3").Where(h => h.GetInnerText() == "Select A Mechanic").Count();
        Assert.That(workerselect == 0);

        cut.SetParametersAndRender(parameters => parameters
        .Add(p => p.SelectedPlane, new PlanesDueForMaintenance() { }));

        workerselect = cut.FindAll("h3").Where(h => h.GetInnerText() == "Select A Mechanic").Count();
        Assert.That(workerselect > 0);
    }

    [Test]
    public void SelectingWorkerAndHangerShowScheduleButton()
    {
        using var ctx = new TestContext();
        ctx.JSInterop.Mode = JSRuntimeMode.Loose;
        var mocked = new Mock<IMaintenanceDataService>();

        ctx.Services.AddScoped(e => mocked.Object);
        var cut = ctx.RenderComponent<MaintenanceSchedulingAssistance>();

        var buttoncount = cut.FindAll("h3").Where(h => h.GetInnerText() == "Schedule Maintenance").Count();
        Assert.That(buttoncount == 0);

        cut.SetParametersAndRender(parameters => parameters
        .Add(p => p.SelectedPlane, new PlanesDueForMaintenance() { })
        .Add(p => p.SelectedMechanic, new CertifiedMechanic() { })
        .Add(p => p.SelectedHanger, new Hanger() { })
        );
        buttoncount = cut.FindAll("span").Where(h => h.GetInnerText() == "Schedule Maintenance").Count();
        Assert.That(buttoncount > 0);
    }
}
