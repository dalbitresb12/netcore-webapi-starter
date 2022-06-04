using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Starter.API.Shared.Utils;

// Adapted from https://stackoverflow.com/a/58406404/15040387
public class RoutePrefixConvention : IApplicationModelConvention {
  private readonly AttributeRouteModel routePrefix;

  public RoutePrefixConvention(IRouteTemplateProvider route) {
    routePrefix = new AttributeRouteModel(route);
  }

  public void Apply(ApplicationModel application) {
    foreach (var selector in application.Controllers.SelectMany(c => c.Selectors)) {
      var routeModel = selector.AttributeRouteModel;
      if (routeModel is null) {
        selector.AttributeRouteModel = routePrefix;
        continue;
      }

      selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(routePrefix, routeModel);
    }
  }
}
