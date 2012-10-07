<?xml version="1.0" encoding="utf-8"?>
<!-- Determines the presentaion of the hidden rendering (when "Hide rendering" action is executed) in Page Editor -->
<xsl:stylesheet version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:sc="http://www.sitecore.net/sc"
  xmlns:sql="http://www.sitecore.net/sql"
  exclude-result-prefixes="sc sql">

  <!-- output directives -->
  <xsl:output method="html" indent="no" encoding="UTF-8"  />

  <!-- sitecore parameters -->
  <xsl:param name="lang" select="'en'"/>
  <xsl:param name="id" select="''"/>
  <xsl:param name="sc_item"/>
  <xsl:param name="sc_currentitem"/>

  <!-- entry point -->
  <xsl:template match="*">
    <div style="background-color: white; opacity: 0.35; filter: alpha(opacity=35);">
      <div style="height:50px;background: transparent url('/sitecore/shell/themes/standard/images/pageeditor/bg_hidden_rendering.png') repeat;">        
      </div>
    </div>
  </xsl:template>

</xsl:stylesheet>
